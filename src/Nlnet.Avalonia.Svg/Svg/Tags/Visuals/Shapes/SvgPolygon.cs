using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;
using SkiaSharp;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polygon))]
public class SvgPolygon : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable, IMarkerable,
    IPointsSetter
{
    public PointList? Points { get; set; }
    public string? MarkerStart
    {
        get;
        set;
    }
    public string? MarkerEnd
    {
        get;
        set;
    }
    public string? MarkerMid
    {
        get;
        set;
    }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        return Points != null ? new PolylineGeometry(Points, true) : null;
    }

    public void RenderMarkerStart(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {

    }

    public void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {

    }

    public void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {

    }
}