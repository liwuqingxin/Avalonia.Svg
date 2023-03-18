using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using Nlnet.Avalonia.Svg.Utils;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.style)]
public class SvgStyleFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgStyle
        {
            Content = xmlNode.InnerText
        };

        if (xmlNode.Attributes != null)
        {
            tag.ContentType = xmlNode.Attributes[SvgProperties.Type]?.Value;
        }

        return tag;
    }
}

public class SvgStyle : SvgTagBase, ISvgStyleProvider
{
    private IList<ISvgStyle>? _styles;

    public string? ContentType { get; set; }
    public string? Content     { get; set; }

    IEnumerable<ISvgStyle> ISvgStyleProvider.GetStyles()
    {
        if (_styles != null)
        {
            return _styles;
        }

        _styles = new List<ISvgStyle>();

        if (string.IsNullOrWhiteSpace(Content))
        {
            return _styles;
        }

        var regex   = new Regex("(.*?)\\{(.*?)\\}", RegexOptions.Singleline);
        var matches = regex.Matches(Content.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
        if (matches.Count <= 0)
        {
            return _styles;
        }

        foreach (Match match in matches)
        {
            if (!match.Success || match.Groups.Count != 3)
            {
                continue;
            }

            var selectorsString = match.Groups[1].Value;
            var settersString   = match.Groups[2].Value;
            var selectors       = GetSelectorsFromString(selectorsString);
            var setters         = GetStyleFromStyleString(settersString);
            if (selectors != null && setters != null)
            {
                _styles.Add(new SvgStyleImpl(selectors, setters));
            }
        }

        return _styles;
    }

    private static IEnumerable<ISvgSetter>? GetStyleFromStyleString(string styleString)
    {
        if (string.IsNullOrWhiteSpace(styleString))
        {
            return null;
        }

        var setters = new List<ISvgSetter>();
        var token = new SafeStringTokenizer(styleString, ';');
        while (true)
        {
            if (!token.TryReadString(out var setterString))
            {
                break;
            }
            if (string.IsNullOrWhiteSpace(setterString?.Trim()))
            {
                continue;
            }
            var name    = setterString[..setterString.IndexOf(':')].Trim();
            var factory = SvgSetterFactory.GetSetterFactory(name);
            if (factory == null)
            {
                continue;
            }
            var setter = factory.CreateSetter();
            var valueString = setterString[(setterString.IndexOf(':') + 1)..].Trim();
            try
            {
                setter.InitializeValue(valueString);
            }
            catch
            {
                setter.AddDeferredValueString(valueString);
            }
            setters.Add(setter);
        }

        if (setters.Count == 0)
        {
            return null;
        }

        return setters;
    }

    private static IEnumerable<IStyleSelector>? GetSelectorsFromString(string selectorString)
    {
        if (string.IsNullOrWhiteSpace(selectorString))
        {
            return null;
        }

        selectorString = selectorString.Trim();
        var selectors = new List<IStyleSelector>();

        for (var i = 0; i < selectorString.Length; i++)
        {
            var ch = selectorString[i];
            var selector = ReadSelector(selectorString, ref i);
            switch (ch)
            {
                case '.':
                selectors.Add(new ClassSelector(selector));
                    break;
                case '#':
                    selectors.Add(new IdSelector(selector));
                    break;
                default:
                    selectors.Add(new TagSelector(selector));
                    break;
            }
        }

        if (selectors.Count == 0)
        {
            return null;
        }

        return selectors;
    }

    private static string ReadSelector(string selectorString, ref int i)
    {
        var start = i;
        i++;
        while (i < selectorString.Length)
        {
            var ch = selectorString[i];
            if (ch is '.' or '#')
            {
                break;
            }
            i++;
        }

        return selectorString.Substring(start, i - start);
    }
}
