using System.Xml;

namespace Avalonia.Svg;

[SvgTag(SvgTags.title)]
public class SvgTitleFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgTitle
        {
            Title = xmlNode.InnerText
        };

        return tag;
    }
}

public class SvgTitle : SvgTagBase
{
    public string? Title { get; set; }
}
