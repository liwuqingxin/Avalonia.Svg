using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;
using SkiaSharp;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polyline))]
public class SvgPolyline : Markerable, ISvgShape, ISvgGraphic, ISvgRenderable, IMarkerable,
    IPointsSetter
{
    public PointList? Points { get; set; }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        return Points != null ? new PolylineGeometry(Points, false) : null;
    }

    public override double GetMarkerOrientDegree(SKPath path, int index)
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
        return Math.Atan((p2.Y - p1.Y) / (p2.X - p1.X));
    }
}