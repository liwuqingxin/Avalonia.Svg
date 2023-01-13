using System.Xml;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.rect)]
public class SvgRectFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgRect();

        if (xmlNode.Attributes != null)
        {
            tag.Id     = xmlNode.Attributes[SvgProperties.Id]?.Value;
            tag.Width  = xmlNode.Attributes[SvgProperties.Width]?.Value;
            tag.Height = xmlNode.Attributes[SvgProperties.Height]?.Value;
            tag.X      = xmlNode.Attributes[SvgProperties.X]?.Value;
            tag.Y      = xmlNode.Attributes[SvgProperties.Y]?.Value;
        }

        return tag;
    }
}

public class SvgRect : SvgTagBase
{
    public string? Id     { get; set; }
    public string? Width  { get; set; }
    public string? Height { get; set; }
    public string? X      { get; set; }
    public string? Y      { get; set; }
}