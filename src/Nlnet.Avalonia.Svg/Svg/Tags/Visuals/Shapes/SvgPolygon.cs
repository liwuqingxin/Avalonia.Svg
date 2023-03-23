using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polygon))]
public class SvgPolygon : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    IPointsSetter
{
    public string? Class { get; set; }

    public PointList? Points { get; set; }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        return Points != null ? new PolylineGeometry(Points, true) : null;
    }
}