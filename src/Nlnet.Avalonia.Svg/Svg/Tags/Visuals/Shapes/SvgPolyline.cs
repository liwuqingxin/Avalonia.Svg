using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
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

        var point = effectivePath.GetPoint(0);

        RenderMarkerOnPoint(dc, ctx, marker, point);
    }

    public void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {
        if (effectivePath.PointCount == 0 || marker.Children == null)
        {
            return;
        }

        var point = effectivePath.LastPoint;

        RenderMarkerOnPoint(dc, ctx, marker, point);
    }

    public void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {
        if (effectivePath.PointCount == 0 || marker.Children == null)
        {
            return;
        }

        for (var i = 1; i < effectivePath.Points.Length - 1; i++)
        {
            var point = effectivePath.Points[i];

            RenderMarkerOnPoint(dc, ctx, marker, point);
        }
    }

    private static void RenderMarkerOnPoint(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPoint point)
    {
        var stack = new Stack<DrawingContext.PushedState>();

        var markerBounds = marker.RenderBounds;
        var x = point.X;
        var y = point.Y;

        using (dc.PushPostTransform(Matrix.CreateTranslation(x, y)))
        {
            using (dc.PushTransformContainer())
            {
                if (marker.MarkerWidth != null && marker.MarkerHeight != null)
                {
                    SvgHelper.GetUniformFactors(new Size(marker.MarkerWidth.Value, marker.MarkerHeight.Value), marker.RenderBounds.Size, false, out var scale, out var offsetX, out var offsetY);

                    stack.Push(dc.PushPostTransform(Matrix.CreateScale(scale, scale)));
                    stack.Push(dc.PushPostTransform(Matrix.CreateTranslation(-markerBounds.Width * scale / 2, -markerBounds.Height * scale / 2)));
                    stack.Push(dc.PushTransformContainer());
                }

                foreach (var child in marker.Children!.OfType<ISvgRenderable>())
                {
                    child.Render(dc, ctx);
                }

                while (stack.Count > 0)
                {
                    var v = stack.Pop() as IDisposable;
                    v.Dispose();
                }
            }
        }
    }
}