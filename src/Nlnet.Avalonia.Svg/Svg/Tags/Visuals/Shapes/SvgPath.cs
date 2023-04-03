using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;
using Point = Avalonia.Point;

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
                var segment   = figure.Segments[0];
                var nextPoint = GetFirstPoint(segment);
                radian = CalculateRadian(point, nextPoint);

                if (figure.IsClosed)
                {
                    var lastSegment   = figure.Segments.Last();
                    var previousPoint = GetLastPoint(lastSegment);
                    var radian2       = CalculateRadian(previousPoint, point);

                    if (double.IsNaN(radian2) == false)
                    {
                        radian = (radian2 + radian) / 2;
                    }
                }
            }

            RenderMarkerOnPoint(dc, ctx, marker, point, radian, true);
        }
    }

    public override void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        if (OriginalGeometry is PathGeometry { Figures.Count: > 0 } path)
        {
            var figure     = path.Figures.Last();
            var firstPoint = figure.StartPoint;
            var radian     = 0d;

            if (figure.Segments is { Count: > 0 })
            {
                var segment = figure.Segments.Last();
                var previousSegment = figure.Segments.Count > 1 ? figure.Segments[^2] : null;
                var lastPoint = GetLastPoint(segment);

                Point? previousPoint = null;
                switch (segment)
                {
                    case ArcSegment s1:
                        if (previousSegment != null)
                        {
                            previousPoint = GetLastPoint(previousSegment);
                        }
                        break;
                    case LineSegment s2:
                        if (previousSegment != null)
                        {
                            previousPoint = GetLastPoint(previousSegment);
                        }
                        break;
                    case PolyLineSegment s3:
                        if (s3.Points.Count > 1)
                        {
                            previousPoint = s3.Points[^2];
                        }
                        else
                        {
                            if (previousSegment != null)
                            {
                                previousPoint = GetLastPoint(previousSegment);
                            }
                        }
                        break;
                    case BezierSegment s4:
                        previousPoint = s4.Point2;
                        break;
                    case QuadraticBezierSegment s5:
                        previousPoint = s5.Point1;
                        break;
                }

                if (previousPoint != null)
                {
                    radian = CalculateRadian(previousPoint.Value, lastPoint);
                }

                if (figure.IsClosed)
                {
                    var radian2 = CalculateRadian(lastPoint, firstPoint);
                    if (double.IsNaN(radian2) == false)
                    {
                        radian = (radian2 + radian) / 2;
                    }
                }

                RenderMarkerOnPoint(dc, ctx, marker, lastPoint, radian, true);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Point GetFirstPoint(PathSegment segment)
    {
        return segment switch
        {
            ArcSegment s => s.Point,
            LineSegment s => s.Point,
            PolyLineSegment s => s.Points.First(),
            BezierSegment s => s.Point1,
            QuadraticBezierSegment s => s.Point1,
            _ => throw new ArgumentOutOfRangeException(nameof(segment), segment, null)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Point GetLastPoint(PathSegment segment)
    {
        return segment switch
        {
            ArcSegment s => s.Point,
            LineSegment s => s.Point,
            PolyLineSegment s => s.Points.Last(),
            BezierSegment s => s.Point3,
            QuadraticBezierSegment s => s.Point2,
            _ => throw new ArgumentOutOfRangeException(nameof(segment), segment, null)
        };
    }

    public override void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        // TODO Implement this later.
    }
}
