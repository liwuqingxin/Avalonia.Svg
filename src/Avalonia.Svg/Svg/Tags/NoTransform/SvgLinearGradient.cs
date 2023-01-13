using System.Linq;
using System.Xml;
using Avalonia.Media;

namespace Avalonia.Svg;

[SvgTag(SvgTags.linearGradient)]
public class SvgLinearGradientFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgLinearGradient();

        if (xmlNode.Attributes != null)
        {
            tag.Id = xmlNode.Attributes[SvgProperties.Id]?.Value            ?? string.Empty;
            tag.X1 = xmlNode.Attributes[SvgProperties.X1]?.Value.ToDouble() ?? 0;
            tag.X2 = xmlNode.Attributes[SvgProperties.X2]?.Value.ToDouble() ?? 1;
            tag.Y1 = xmlNode.Attributes[SvgProperties.Y1]?.Value.ToDouble() ?? 0;
            tag.Y2 = xmlNode.Attributes[SvgProperties.Y2]?.Value.ToDouble() ?? 0;
        }

        return tag;
    }
}

public class SvgLinearGradient : SvgTagBase, ISvgBrushProvider
{
    private IBrush? _brush;

    public string Id { get; set; }
    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }

    string ISvgBrushProvider.Id
    {
        get => this.Id;
        set => this.Id = value;
    }

    IBrush ISvgBrushProvider.GetBrush()
    {
        if (_brush != null)
        {
            return _brush;
        }

        var brush = new LinearGradientBrush
        {
            StartPoint = new RelativePoint(X1, Y1, RelativeUnit.Absolute),
            EndPoint   = new RelativePoint(X2, Y2, RelativeUnit.Absolute)
        };

        if (this.Children != null)
        {
            brush.GradientStops.AddRange(this.Children.OfType<SvgStop>().Select(s => s.GradientStop));
        }

        _brush = brush;
        return _brush;
    }
}