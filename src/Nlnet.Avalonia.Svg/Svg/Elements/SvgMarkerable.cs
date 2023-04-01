using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.Utils;
using SkiaSharp;

namespace Nlnet.Avalonia.Svg;

public abstract class SvgMarkerable : SvgShape, ISvgMarkerable
{
    public string? MarkerStart { get; set; }
    public string? MarkerEnd   { get; set; }
    public string? MarkerMid   { get; set; }
    public string? Marker      { get; set; }


    public void RenderMarkerStart(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {
        if (effectivePath.PointCount == 0 || marker.Children == null)
        {
            return;
        }

        var radians = GetMarkerOrientRadians(effectivePath, 0);
        WithOrientMode(marker, true, ref radians);
        var point = effectivePath.GetPoint(0);
        RenderMarkerOnPoint(dc, ctx, marker, point, radians);
    }

    public void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {
        if (effectivePath.PointCount == 0 || marker.Children == null)
        {
            return;
        }

        var radians = GetMarkerOrientRadians(effectivePath, effectivePath.PointCount - 1);
        WithOrientMode(marker, false, ref radians);
        var point = effectivePath.LastPoint;
        RenderMarkerOnPoint(dc, ctx, marker, point, radians);
    }

    public void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath)
    {
        if (effectivePath.PointCount == 0 || marker.Children == null)
        {
            return;
        }

        List<int>? effectivePoints = null;
        if (OriginalGeometry is PathGeometry pathGeometry)
        {
            effectivePoints = GetEffectivePointsIndexFromOriginGeometry(pathGeometry);
        }

        // TODO Not well here.

        if (effectivePoints != null)
        {
            for (var i = 1; i < effectivePoints.Count - 1; i++)
            {
                var point   = effectivePath.Points[effectivePoints[i]];
                var radians = GetMarkerOrientRadians(effectivePath, i);
                WithOrientMode(marker, false, ref radians);
                RenderMarkerOnPoint(dc, ctx, marker, point, radians);
            }
        }
        else
        {
            for (var i = 1; i < effectivePath.Points.Length - 1; i++)
            {
                var point = effectivePath.Points[i]; 
                var radians = GetMarkerOrientRadians(effectivePath, i);
                WithOrientMode(marker, false, ref radians);
                RenderMarkerOnPoint(dc, ctx, marker, point, radians);
            }
        }
    }

    private static List<int> GetEffectivePointsIndexFromOriginGeometry(PathGeometry pathGeometry)
    {
        var list = new List<int>();
        if (pathGeometry.Figures == null)
        {
            return list;
        }
        var index = 0;
        list.Add(index);
        foreach (var figure in pathGeometry.Figures)
        {
            if (figure.Segments == null)
            {
                continue;
            }
            foreach (var segment in figure.Segments)
            {
                switch (segment)
                {
                    case LineSegment line:
                        index++;
                        list.Add(index);
                        break;
                    case PolyLineSegment polyLine:
                        for (var i = 0; i < polyLine.Points.Count; i++)
                        {
                            index++;
                            list.Add(index);
                        }
                        break;
                    case ArcSegment arc:
                        index++;
                        list.Add(index);
                        break;
                    case BezierSegment bezier:
                        index += 3;
                        list.Add(index);
                        break;
                    case QuadraticBezierSegment quadraticBezier:
                        index += 2;
                        list.Add(index);
                        break;
                }
            }
        }

        return list;
    }

    private static void WithOrientMode(SvgMarker marker, bool isFirstPoint, ref double angle)
    {
        if (marker.MarkerOrient?.Mode == SvgMarkerOrientMode.auto_start_reverse && isFirstPoint)
        {
            angle -= Math.PI;
        }
        else if (marker.MarkerOrient?.Mode == SvgMarkerOrientMode.angle)
        {
            angle = marker.MarkerOrient.Angle;
        }
    }

    private void RenderMarkerOnPoint(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPoint point, double radians)
    {
        var stack = new Stack<DrawingContext.PushedState>();

        // 1. Move to point and rotate it.
        var x = point.X;
        var y = point.Y;
        stack.Push(dc.PushPostTransform(Matrix.CreateTranslation(x, y)));
        stack.Push(dc.PushTransformContainer());
        stack.Push(dc.PushPostTransform(MatrixUtil.CreateRotationRadians(radians)));
        stack.Push(dc.PushTransformContainer());

        // 2. Apply unit.
        if (marker.MarkerUnits == SvgMarkerUnits.strokeWidth)
        {
            var strokeWidth = this.GetPropertyStructValue<IStrokeWidthSetter, double>();
            stack.Push(dc.PushPostTransform(Matrix.CreateScale(strokeWidth, strokeWidth)));
            stack.Push(dc.PushTransformContainer());
        }

        // 3. Get the real size of marker.
        var markerWidth  = marker.MarkerWidth  ?? 3;
        var markerHeight = marker.MarkerHeight ?? 3;

        // 4. Clip it.
        //stack.Push(dc.PushClip(new Rect(new Size(markerWidth, markerHeight))));

        // 5. Scale the ViewBox.
        if (marker.ViewBox != null)
        {
            SvgHelper.GetUniformFactors(new Size(markerWidth, markerHeight), new Size(marker.ViewBox.Width, marker.ViewBox.Height), false, out var scale, out var offsetX, out var offsetY);
            stack.Push(dc.PushPostTransform(Matrix.CreateScale(scale, scale)));
            stack.Push(dc.PushPostTransform(Matrix.CreateTranslation(-offsetX, -offsetY)));
            stack.Push(dc.PushTransformContainer());
        }

        // 6. Move by RefX and RefY.
        var refX = marker.RefX?.Get(markerWidth)  ?? 0;
        var refY = marker.RefY?.Get(markerHeight) ?? 0;
        stack.Push(dc.PushPostTransform(Matrix.CreateTranslation(-refX, -refY)));
        stack.Push(dc.PushTransformContainer());

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

    protected virtual double GetMarkerOrientRadians(SKPath path, int index)
    {
        if (path.PointCount is 0 or 1)
        {
            return 0;
        }

        if (index == 0)
        {
            var point1 = path.Points[0];
            var point2 = path.Points[1];
            return GetAngle(point1, point2);
        }
        else if (index == path.PointCount - 1)
        {
            var point1 = path.Points[index - 1];
            var point2 = path.Points[index];
            return GetAngle(point1, point2);
        }
        else
        {
            var point1 = path.Points[index - 1];
            var point2 = path.Points[index];
            var point3 = path.Points[index + 1];
            return (GetAngle(point1, point2) + GetAngle(point2, point3)) / 2;
        }
    }

    private static double GetAngle(SKPoint p1, SKPoint p2)
    {
        var angle = Math.Atan((p2.Y - p1.Y) / (p2.X - p1.X));
        if (p2.X < p1.X)
        {
            angle += Math.PI;
        }
        return angle;
    }
}
