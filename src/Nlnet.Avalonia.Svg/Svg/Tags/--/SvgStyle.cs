using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

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
            var styleImpl = SvgStyleImpl.Parse(selectorsString, settersString);
            if (styleImpl != null)
            {
                _styles.Add(styleImpl);
            }
        }

        return _styles;
    }
}
