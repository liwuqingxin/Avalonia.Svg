using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Nlnet.Avalonia.Svg.CompileGenerator;
using SkiaSharp;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polyline))]
public class SvgPolyline : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable, IMarkerable,
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
        return Points != null ? new PolylineGeometry(Points, false) : null;
    }

    public void RenderMarkerStart(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {
        if (effectivePath.PointCount == 0 || marker.Children == null)
        {
            return;
        }

        var point        = effectivePath.GetPoint(0);
        var markerBounds = marker.RenderBounds;
        var x            = point.X - markerBounds.Width  / 2;
        var y            = point.Y - markerBounds.Height / 2;

        RenderMarkerOnPoint(dc, ctx, marker, new Point(x, y));
    }

    public void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {
        if (effectivePath.PointCount == 0 || marker.Children == null)
        {
            return;
        }

        var point        = effectivePath.LastPoint;
        var markerBounds = marker.RenderBounds;
        var x            = point.X - markerBounds.Width  / 2;
        var y            = point.Y - markerBounds.Height / 2;

        RenderMarkerOnPoint(dc, ctx, marker, new Point(x, y));
    }

    public void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {
        if (effectivePath.PointCount == 0 || marker.Children == null)
        {
            return;
        }

        for (var i = 1; i < effectivePath.Points.Length - 1; i++)
        {
            var point        = effectivePath.Points[i];
            var markerBounds = marker.RenderBounds;
            var x            = point.X - markerBounds.Width  / 2;
            var y            = point.Y - markerBounds.Height / 2;

            RenderMarkerOnPoint(dc, ctx, marker, new Point(x, y));
        }
    }

    private static void RenderMarkerOnPoint(DrawingContext dc, ISvgContext ctx, SvgMarker marker, Point point)
    {
        using (dc.PushPostTransform(Matrix.CreateTranslation(point.X, point.Y)))
        {
            using (dc.PushTransformContainer())
            {
                foreach (var child in marker.Children!.OfType<ISvgRenderable>())
                {
                    child.Render(dc, ctx);
                }
            }
        }
    }
}