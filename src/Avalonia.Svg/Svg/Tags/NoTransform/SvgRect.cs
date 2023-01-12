using System.Xml;

namespace Avalonia.Svg;

[SvgTag(SvgTags.rect)]
public class SvgRectFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgRect();

        if (xmlNode.Attributes != null)
        {
            tag.Id     = xmlNode.Attributes["id"]?.Value;
            tag.Width  = xmlNode.Attributes["width"]?.Value;
            tag.Height = xmlNode.Attributes["height"]?.Value;
            tag.X      = xmlNode.Attributes["x"]?.Value;
            tag.Y      = xmlNode.Attributes["y"]?.Value;
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