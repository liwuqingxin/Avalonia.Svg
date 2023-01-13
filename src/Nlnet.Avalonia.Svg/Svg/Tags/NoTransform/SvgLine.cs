using System.Xml;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.line)]
public class SvgLineFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgLine();

        if (xmlNode.Attributes != null)
        {
            tag.Class = xmlNode.Attributes[SvgProperties.Class]?.Value;
            tag.X1    = xmlNode.Attributes[SvgProperties.X1]?.Value;
            tag.X2    = xmlNode.Attributes[SvgProperties.X2]?.Value;
            tag.Y1    = xmlNode.Attributes[SvgProperties.Y1]?.Value;
            tag.Y2    = xmlNode.Attributes[SvgProperties.Y2]?.Value;
        }

        return tag;
    }
}

public class SvgLine : SvgTagBase
{
    public string? Class { get; set; }
    public string? X1    { get; set; }
    public string? X2    { get; set; }
    public string? Y1    { get; set; }
    public string? Y2    { get; set; }
}