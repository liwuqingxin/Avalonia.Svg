using System.Xml;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.stop)]
public class SvgStopFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        if (xmlNode.Attributes != null)
        {
            // ref https://www.w3.org/TR/SVG2/pservers.html#StopElementOffsetAttribute
            var offset      = xmlNode.Attributes[SvgProperties.Offset]?.Value.ToDouble()      ?? 0;
            var stopOpacity = xmlNode.Attributes[SvgProperties.StopOpacity]?.Value.ToDouble() ?? 1;
            var stopColor   = Color.Parse(xmlNode.Attributes[SvgProperties.StopColor]?.Value ?? "Black");
            return new SvgStop(offset, stopOpacity, stopColor);
        }
        else
        {
            return new SvgStop(0, 1, Colors.Black);
        }
    }
}

public class SvgStop : SvgTagBase
{
    public GradientStop GradientStop { get; set; }

    public SvgStop(double offset, double opacity, Color color)
    {
        GradientStop = new GradientStop
        {
            //
            // NOTE Opacity of GradientStop is not supported in Avalonia 11.0.0-preview4.
            // So we apply it to the color.
            //
            Color = Color.FromArgb((byte)(opacity * color.A), color.R, color.G, color.B),
            Offset = offset,
        };
    }
}