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
    private IList<ISvgClassStyle>? _styles;

    public string? ContentType { get; set; }
    public string? Content     { get; set; }

    IEnumerable<ISvgClassStyle> ISvgStyleProvider.GetStyles()
    {
        if (_styles != null)
        {
            return _styles;
        }

        _styles = new List<ISvgClassStyle>();

        if (string.IsNullOrWhiteSpace(Content))
        {
            return _styles;
        }

        var regex   = new Regex("\\.(.*?)\\{(.*?)\\}", RegexOptions.Singleline);
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

            var name  = match.Groups[1].Value;
            var value = match.Groups[2].Value;
            var style = GetStyleFromStyleString(name, value);
            if (style != null)
            {
                _styles.Add(style);
            }
        }

        return _styles;
    }

    private static ISvgClassStyle? GetStyleFromStyleString(string @class, string styleString)
    {
        if (string.IsNullOrWhiteSpace(styleString))
        {
            return null;
        }

        var setters = new List<ISvgStyleSetter>();
        var token = new SafeStringTokenizer(styleString, ';');
        while (true)
        {
            if (!token.TryReadString(out var setterString))
            {
                break;
            }
            var name    = setterString![..setterString!.IndexOf(':')];
            var factory = SvgStyleSetterFactory.GetSetterFactory(name);
            if (factory == null)
            {
                continue;
            }
            var setter = factory.CreateSetter();
            var valueString = setterString[(setterString.IndexOf(':') + 1)..];
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

        ISvgClassStyle style = new SvgClassStyle(@class, setters);
        return style;
    }
}
