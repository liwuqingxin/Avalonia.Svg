using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.linearGradient))]
public class SvgLinearGradient : SvgTagBase, ISvgPaintServer, ISvgBrushProvider,
    IX1Setter, 
    IX2Setter, 
    IY1Setter, 
    IY2Setter,
    IGradientSpreadMethodSetter,
    IGradientUnitsSetter
{
    private LightBrush? _brush;

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
    public GradientSpreadMethod? GradientSpreadMethod
    {
        get;
        set;
    }
    public GradientUnit? GradientUnits
    {
        get;
        set;
    }

    string? ISvgBrushProvider.Id
    {
        get => ((IIdSetter)this).Id;
        set => ((IIdSetter)this).Id = value;
    }

    LightBrush ISvgBrushProvider.GetBrush()
    {
        if (_brush != null)
        {
            return _brush;
        }

        // ref https://www.w3.org/TR/SVG2/pservers.html#LinearGradientElementX1Attribute
        var gradientBrush = new LightLinearGradientBrush(
            gradientStops: Children?.OfType<SvgStop>().Select(s => s.GradientStop).ToList() ?? new List<ImmutableGradientStop>(),
            opacity: 1,
            transform: null,
            transformOrigin: null,
            spreadMethod: GradientSpreadMethod ?? global::Avalonia.Media.GradientSpreadMethod.Pad,
            startPoint: new RelativePoint(X1 ?? 0, Y1 ?? 0, RelativeUnit.Relative),
            endPoint: new RelativePoint(X2   ?? 1, Y2 ?? 0, RelativeUnit.Relative))
        {
            Unit = GradientUnits ?? GradientUnit.objectBoundingBox
        };

        return _brush = gradientBrush;
    }
}