using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;
using SkiaSharp;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable, IMarkerable,
    IDataSetter
{
    public Geometry? Data { get; set; }
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
        return Data;
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
