using System.Linq;
using Avalonia;
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
        var fillRule = this.GetPropertyStructValue<IFillRuleSetter, FillRule>();
        if (Data is PathGeometry path)
        {
            path.FillRule = fillRule;
        }

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
                var segment = figure.Segments[0];
                radian = segment switch
                {
                    ArcSegment s1 => CalculateRadian(point, s1.Point),
                    LineSegment s2 => CalculateRadian(point, s2.Point),
                    PolyLineSegment s3 => CalculateRadian(point, s3.Points[0]),
                    BezierSegment s4 => CalculateRadian(point, s4.Point1),
                    QuadraticBezierSegment s5 => CalculateRadian(point, s5.Point1),
                    _ => radian
                };

                if (figure.IsClosed)
                {
                    var lastSegment = figure.Segments.Last();
                    var radian2 = lastSegment switch
                    {
                        ArcSegment s1 => CalculateRadian(s1.Point, point),
                        LineSegment s2 => CalculateRadian(s2.Point, point),
                        PolyLineSegment s3 => CalculateRadian(s3.Points.Last(), point),
                        BezierSegment s4 => CalculateRadian(s4.Point3, point),
                        QuadraticBezierSegment s5 => CalculateRadian(s5.Point2, point),
                        _ => radian
                    };

                    radian = (radian2 + radian) / 2;
                }
            }

            RenderMarkerOnPoint(dc, ctx, marker, point, radian, true);
        }
    }

    public override void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        这里
        // TODO
    }

    public override void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        // TODO
    }
}
