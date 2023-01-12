using System.Xml;

namespace Avalonia.Svg;

[SvgTag(SvgTags.ellipse)]
public class SvgEllipseFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgEllipse();

        if (xmlNode.Attributes != null)
        {
            tag.Id = xmlNode.Attributes["id"]?.Value;
            tag.RX = xmlNode.Attributes["rx"]?.Value;
            tag.RY = xmlNode.Attributes["ry"]?.Value;
            tag.CX = xmlNode.Attributes["cx"]?.Value;
            tag.CY = xmlNode.Attributes["cy"]?.Value;
            tag.Opacity = xmlNode.Attributes["opacity"]?.Value;
            tag.Fill = xmlNode.Attributes["fill"]?.Value;
        }

        return tag;
    }
}

public class SvgEllipse : SvgTagBase
{
    public string? Id { get; set; }
    public string? RX { get; set; }
    public string? RY { get; set; }
    public string? CX { get; set; }
    public string? CY { get; set; }
    public string? Opacity { get; set; }
    public string? Fill { get; set; }
}