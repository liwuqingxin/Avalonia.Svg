using System.Xml;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.use)]
public class SvgUseFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgUse();

        if (xmlNode.Attributes != null)
        {
            tag.Style = xmlNode.Attributes[SvgProperties.Style]?.Value;
            tag.Href  = xmlNode.Attributes[SvgProperties.Href]?.Value;
        }

        return tag;
    }
}

public class SvgUse : SvgTagBase
{
    public string? Style { get; set; }
    public string? Href  { get; set; }
}
