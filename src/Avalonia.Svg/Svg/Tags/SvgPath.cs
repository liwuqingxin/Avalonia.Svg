using System.Xml;
using Avalonia.Media;
using Avalonia.Svg.Setters;

namespace Avalonia.Svg;

[SvgTag(SvgTags.path)]
public class SvgPathFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        if (xmlNode.Attributes?[SvgProperties.Data]?.Value == null)
        {
            return SvgIgnore.Default;
        }

        var svgPath = new SvgPath();
        svgPath.FetchProperties(xmlNode.Attributes!);
        
        return svgPath;
    }
}

public class SvgPath : SvgTagBase, ISvgVisual, 
    IClassSetter, 
    IDataSetter,  
    IFillSetter, 
    IStrokeSetter, 
    IStrokeWidthSetter, 
    IOpacitySetter
{
    public Geometry? RenderGeometry { get; private set; }

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

    Rect ISvgVisual.Bounds => Data?.Bounds ?? Rect.Empty;

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
        if (Data == null)
        {
            return;
        }

        RenderGeometry = Data.Clone();
        RenderGeometry.Transform = transform;
    }

    public string?   Class       { get; set; }
    public Geometry? Data        { get; set; }
    public IBrush?   Fill        { get; set; }
    public IBrush?   Stroke      { get; set; }
    public double?   StrokeWidth { get; set; }
    public double?   Opacity     { get; set; }
}
