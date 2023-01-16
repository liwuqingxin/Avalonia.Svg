using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polygon))]
public class SvgPolygon : SvgTagBase,
    ISvgVisual,
    IClassSetter,
    IPointsSetter
{
    private Rect? _rect;

    public string?    Class        { get; set; }
    public IBrush?    Fill         { get; set; }
    public IBrush?    Stroke       { get; set; }
    public double?    StrokeWidth  { get; set; }
    public double?    Opacity      { get; set; }
    public PointList? Points       { get; set; }
    public PointList? RenderPoints { get; set; }

    public Rect Bounds
    {
        get
        {
            if (_rect != null)
            {
                return _rect.Value;
            }

            if (Points == null)
            {
                return (_rect = Rect.Empty).Value;
            }

            var minX = 0d;
            var minY = 0d;
            var maxX = 0d;
            var maxY = 0d;
            foreach (var point in Points)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }

            _rect = new Rect(minX, minY, maxX - minX, maxY - minY);
            return _rect.Value;
        }
    }

    public Rect RenderBounds { get; set; }

    public SvgPolygon()
    {
        ResourceAppliers = new List<ISvgResourceApplier>()
        {
            new ClassApplier(),
            new DeferredPropertiesApplier(),
        };
    }

    public void Render(DrawingContext dc)
    {
        if (RenderPoints is not {Count: > 1})
        {
            return;
        }

        var polyLineGeometry = new PolylineGeometry(RenderPoints, true);

        dc.RenderWithOpacity(Opacity, () =>
        {
            dc.DrawGeometry(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), polyLineGeometry);
        });
    }

    public void ApplyTransform(Transform transform)
    {
        RenderBounds = Bounds.TransformToAABB(transform.Value);

        if (Points == null)
        {
            return;
        }

        RenderPoints = new PointList();

        foreach (var point in Points)
        {
            RenderPoints.Add(new Point(point.X + transform.Value.M31, point.Y + transform.Value.M32));
        }
    }
}