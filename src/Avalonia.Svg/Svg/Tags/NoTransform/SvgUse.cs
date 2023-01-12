using System.Xml;

namespace Avalonia.Svg;

[SvgTag(SvgTags.use)]
public class SvgUseFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgUse();

        if (xmlNode.Attributes != null)
        {
            tag.Style = xmlNode.Attributes["style"]?.Value;
            tag.Href  = xmlNode.Attributes["xlink:href"]?.Value;
        }

        return tag;
    }
}

public class SvgUse : SvgTagBase
{
    public string? Style { get; set; }
    public string? Href  { get; set; }
}
