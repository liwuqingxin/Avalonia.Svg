using System;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.Utils;

namespace Nlnet.Avalonia.Svg;

public abstract class SvgMarkerable : SvgShape, ISvgMarkerable
{
    public string? MarkerStart { get; set; }
    public string? MarkerEnd   { get; set; }
    public string? MarkerMid   { get; set; }
    public string? Marker      { get; set; }

    protected static double CalculateRadian(Point p1, Point p2)
    {
        var angle = Math.Atan((p2.Y - p1.Y) / (p2.X - p1.X));
        if (p2.X < p1.X)
        {
            angle += Math.PI;
        }
        return angle;
    }

    protected static double CalculateRadian(Point p1, Point p2, Point p3)
    {
        var angle1 = Math.Atan((p2.Y - p1.Y) / (p2.X - p1.X));
        if (p2.X < p1.X)
        {
            angle1 += Math.PI;
        }

        var angle2 = Math.Atan((p3.Y - p2.Y) / (p3.X - p2.X));
        if (p3.X < p2.X)
        {
            angle2 += Math.PI;
        }

        return (angle1 + angle2) / 2;
    }

    private static void WithOrientMode(IMarkerOrientSetter orientSetter, bool isFirstPoint, ref double radian)
    {
        if (orientSetter.MarkerOrient?.Mode == SvgMarkerOrientMode.auto_start_reverse && isFirstPoint)
        {
            radian -= Math.PI;
        }
        else if (orientSetter.MarkerOrient?.Mode == SvgMarkerOrientMode.angle)
        {
            radian = orientSetter.MarkerOrient.Angle;
        }
    }

    protected void RenderMarkerOnPoint(DrawingContext dc, ISvgContext ctx, SvgMarker marker, Point point, double radian, bool isStart)
    {
        WithOrientMode(marker, isStart, ref radian);

        using var stack = new StateStack();

        // 1. Move to point and rotate it.
        var x = point.X;
        var y = point.Y;
        stack.Push(dc.PushPostTransform(Matrix.CreateTranslation(x, y)));
        stack.Push(dc.PushTransformContainer());
        stack.Push(dc.PushPostTransform(MatrixUtil.CreateRotationRadians(radian)));
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
    }

    public abstract void RenderMarkerStart(DrawingContext dc, ISvgContext ctx, SvgMarker marker);

    public abstract void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker);

    public abstract void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker);
}
