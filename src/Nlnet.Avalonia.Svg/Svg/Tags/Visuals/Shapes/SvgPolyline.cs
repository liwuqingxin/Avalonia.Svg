using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polyline))]
public class SvgPolyline : SvgMarkerable, ISvgShape, ISvgGraphic, ISvgRenderable, ISvgMarkerable,
    IPointsSetter
{
    public PointList? Points { get; set; }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        return Points != null ? new PolylineGeometry(Points, false) : null;
    }

    public override void RenderMarkerStart(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        if (OriginalGeometry is PolylineGeometry polyline && polyline.Points.Count > 0)
        {
            var point = polyline.Points.First();
            EnsureTransform(ref point);
            var radian = 0d;
            if (polyline.Points.Count > 1)
            {
                var nextPoint = polyline.Points[1];
                EnsureTransform(ref nextPoint);
                radian = CalculateRadian(point, nextPoint);
            }
            RenderMarkerOnPoint(dc, ctx, marker, point, radian, true);
        }
    }

    public override void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        if (OriginalGeometry is PolylineGeometry polyline && polyline.Points.Count > 0)
        {
            var point = polyline.Points.Last();
            EnsureTransform(ref point);
            var radian = 0d;
            if (polyline.Points.Count > 1)
            {
                var lastPoint = polyline.Points[^2];
                EnsureTransform(ref lastPoint);
                radian = CalculateRadian(lastPoint, point);
            }
            RenderMarkerOnPoint(dc, ctx, marker, point, radian, false);
        }
    }

    public override void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        if (OriginalGeometry is PolylineGeometry polyline && polyline.Points.Count > 2)
        {
            for (var i = 1; i < polyline.Points.Count - 1; i++)
            {
                var p1 = polyline.Points[i - 1];
                var p2 = polyline.Points[i];
                var p3 = polyline.Points[i + 1];
                EnsureTransform(ref p1);
                EnsureTransform(ref p2);
                EnsureTransform(ref p3);

                var radian = CalculateRadian(p1, p2, p3);
                RenderMarkerOnPoint(dc, ctx, marker, p2, radian, false);
            }
        }
    }
}