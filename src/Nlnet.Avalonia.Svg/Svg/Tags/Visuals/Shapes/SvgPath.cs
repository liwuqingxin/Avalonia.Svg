using System.Linq;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : SvgMarkerable, ISvgShape, ISvgGraphic, ISvgRenderable, ISvgMarkerable,
    IDataSetter
{
    public Geometry? Data { get; set; }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        return Data;
    }

    public override void RenderMarkerStart(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        if (OriginalGeometry is PathGeometry { Figures.Count: > 0 } path)
        {
            var figure = path.Figures[0];
            var point  = figure.StartPoint;
            var radian = 0d;

            if (figure.Segments is { Count: > 0 })
            {

            }
            var segment = figure.Segments[0];
            
            RenderMarkerOnPoint(dc, ctx, marker, point, radian, true);
        }
    }

    public override void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        if (OriginalGeometry is PolylineGeometry polyline && polyline.Points.Count > 0)
        {
            var point  = polyline.Points.Last();
            var radian = 0d;
            if (polyline.Points.Count > 1)
            {
                var lastPoint = polyline.Points[^2];
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
                var p1     = polyline.Points[i - 1];
                var p2     = polyline.Points[i];
                var p3     = polyline.Points[i + 1];
                var radian = CalculateRadian(p1, p2, p3);
                RenderMarkerOnPoint(dc, ctx, marker, p2, radian, false);
            }
        }
    }
}
