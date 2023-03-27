using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.circle))]
public class SvgCircle : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    ICXSetter,
    ICYSetter,
    IRSetter
{
    public double? CX { get; set; }

    public double? CY { get; set; }

    public double? R { get; set; }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        if (R != null)
        {
            var cx = CX ?? 0;
            var cy = CY ?? 0;
            return new EllipseGeometry(new Rect(cx - R.Value, cy - R.Value, R.Value * 2, R.Value * 2));
        }

        return null;
    }
}