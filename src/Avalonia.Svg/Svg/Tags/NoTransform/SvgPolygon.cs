using System.Xml;

namespace Avalonia.Svg;

[SvgTag(SvgTags.polygon)]
public class SvgPolygonFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgPolygon();

        if (xmlNode.Attributes != null)
        {
            tag.Class = xmlNode.Attributes["class"]?.Value;
            tag.Points = xmlNode.Attributes["points"]?.Value;
        }

        return tag;
    }
}

public class SvgPolygon : SvgTagBase
{
    public string? Class { get; set; }
    public string? Points { get; set; }
}