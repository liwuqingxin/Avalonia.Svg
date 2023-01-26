using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.linearGradient))]
public class SvgLinearGradient : SvgTagBase, ISvgBrushProvider, IIdSetter, IX1Setter, IX2Setter, IY1Setter, IY2Setter
{
    private IBrush? _brush;

    public string? Id
    {
        get;
        set;
    }
    public double? X1
    {
        get;
        set;
    }
    public double? X2
    {
        get;
        set;
    }
    public double? Y1
    {
        get;
        set;
    }
    public double? Y2
    {
        get;
        set;
    }

    string? ISvgBrushProvider.Id
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
            // ref https://www.w3.org/TR/SVG2/pservers.html#LinearGradientElementX1Attribute
            StartPoint = new RelativePoint(X1 ?? 0, Y1 ?? 0, RelativeUnit.Relative),
            EndPoint   = new RelativePoint(X2 ?? 1, Y2 ?? 0, RelativeUnit.Relative)
        };

        if (this.Children != null)
        {
            brush.GradientStops.AddRange(this.Children.OfType<SvgStop>().Select(s => s.GradientStop));
        }

        _brush = brush;
        return _brush;
    }
}