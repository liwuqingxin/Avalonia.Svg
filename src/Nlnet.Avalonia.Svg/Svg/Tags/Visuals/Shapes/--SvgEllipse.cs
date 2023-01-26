using System.Xml;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.ellipse)]
public class SvgEllipseFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgEllipse();

        if (xmlNode.Attributes != null)
        {
            tag.Id      = xmlNode.Attributes[SvgProperties.Id]?.Value;
            tag.RX      = xmlNode.Attributes[SvgProperties.RX]?.Value;
            tag.RY      = xmlNode.Attributes[SvgProperties.RY]?.Value;
            tag.CX      = xmlNode.Attributes[SvgProperties.CX]?.Value;
            tag.CY      = xmlNode.Attributes[SvgProperties.CY]?.Value;
            tag.Opacity = xmlNode.Attributes[SvgProperties.Opacity]?.Value;
            tag.Fill    = xmlNode.Attributes[SvgProperties.Fill]?.Value;
        }

        return tag;
    }
}

public class SvgEllipse : SvgTagBase, IShape, IGraphic, IRenderable
{
    public string? Id { get; set; }
    public string? RX { get; set; }
    public string? RY { get; set; }
    public string? CX { get; set; }
    public string? CY { get; set; }
    public string? Opacity { get; set; }
    public string? Fill { get; set; }
}