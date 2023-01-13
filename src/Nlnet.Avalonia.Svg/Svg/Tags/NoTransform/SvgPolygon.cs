using System.Xml;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.polygon)]
public class SvgPolygonFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgPolygon();

        if (xmlNode.Attributes != null)
        {
            tag.Class = xmlNode.Attributes[SvgProperties.Class]?.Value;
            tag.Points = xmlNode.Attributes[SvgProperties.Points]?.Value;
        }

        return tag;
    }
}

public class SvgPolygon : SvgTagBase
    //ISvgVisual,
    //IClassSetter,
    //IPointsSetter,
{
    public string? Class { get; set; }
    public string? Points { get; set; }
}