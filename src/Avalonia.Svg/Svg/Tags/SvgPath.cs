using System.Xml;
using Avalonia.Media;
using Avalonia.Svg.Setters;

namespace Avalonia.Svg;

[SvgTag(SvgTags.path)]
public class SvgPathFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var data = xmlNode.Attributes?[SvgProperties.Data]?.Value;
        if (data == null)
        {
            return SvgIgnore.Default;
        }

        var svgPath = new SvgPath(data.ToGeometry());
        var attrs = xmlNode.Attributes!;

        svgPath.Class = attrs[SvgProperties.Class]?.Value;
        svgPath.FetchProperties(attrs);
        
        return svgPath;
    }
}

public class SvgPath : SvgTagBase, ISvgVisual, IFillSetter, IStrokeSetter, IStrokeWidthSetter, IOpacitySetter
{
    public string?   Class          { get; internal set; }
    public Geometry  Geometry       { get; }
    public Geometry? RenderGeometry { get; private set; }

    public SvgPath(Geometry geometry)
    {
        Geometry = geometry;
    }

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

    Rect ISvgVisual.Bounds => Geometry.Bounds;

    Rect ISvgVisual.RenderBounds => RenderGeometry?.Bounds ?? Rect.Empty;

    void ISvgVisual.Render(DrawingContext dc)
    {
        if (RenderGeometry == null)
        {
            return;
        }

        if (Opacity != null)
        {
            using (dc.PushOpacity(Opacity.Value))
            {
                dc.DrawGeometry(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), RenderGeometry);
            }
        }
        else
        {
            dc.DrawGeometry(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), RenderGeometry);
        }
    }

    void ISvgVisual.ApplyTransform(Transform transform)
    {
        Geometry.Transform = transform;
        RenderGeometry = Geometry;
    }

    public IBrush? Fill        { get; set; }
    public IBrush? Stroke      { get; set; }
    public double? StrokeWidth { get; set; }
    public double? Opacity     { get; set; }
}
