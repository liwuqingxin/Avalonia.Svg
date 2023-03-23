using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polyline))]
public class SvgPolyline : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    IPointsSetter
{
    public string? Class { get; set; }

    public PointList? Points { get; set; }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        return Points != null ? new PolylineGeometry(Points, false) : null;
    }
}