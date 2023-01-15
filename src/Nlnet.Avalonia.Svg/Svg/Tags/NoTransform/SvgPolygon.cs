using System;
using System.Linq;
using System.Xml;
using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.polygon)]
public class SvgPolygonFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgPolygon();
        xmlNode.Attributes?.FetchPropertiesTo(tag);
        return tag;
    }
}

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

    public override void ApplyResources(ISvgResourceCollector collector)
    {
        if (!string.IsNullOrWhiteSpace(Class))
        {
            if (collector.Styles.TryGetValue(Class, out var style))
            {
                style.ApplyTo(this);
            }
        }

        if (DeferredProperties == null)
        {
            return;
        }

        foreach (var pair in DeferredProperties)
        {
            var setter = SvgStyleSetterFactory.GetSetterFactory(pair.Key)?.CreateSetter();
            if (setter == null)
            {
                continue;
            }
            setter.InitializeDeferredValue(collector, pair.Value);
            setter.Set(this);
        }
    }

    public void Render(DrawingContext dc)
    {
        if (RenderPoints is not {Count: > 1})
        {
            return;
        }

        var polyLineGeometry = new PolylineGeometry(RenderPoints, true);

        if (Opacity != null)
        {
            using (dc.PushOpacity(Opacity.Value))
            {
                dc.DrawGeometry(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), polyLineGeometry);
            }
        }
        else
        {
            dc.DrawGeometry(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), polyLineGeometry);
        }
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