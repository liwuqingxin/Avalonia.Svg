using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : SvgTagBase, 
    ISvgVisual, 
    IClassSetter, 
    IDataSetter
{
    private Geometry? RenderGeometry { get; set; }

    public string?   Class       { get; set; }
    public Geometry? Data        { get; set; }
    public IBrush?   Fill        { get; set; }
    public IBrush?   Stroke      { get; set; }
    public double?   StrokeWidth { get; set; }
    public double?   Opacity     { get; set; }

    public SvgPath()
    {
        ResourceAppliers = new List<ISvgResourceApplier>()
        {
            new ClassApplier(),
            new DeferredPropertiesApplier(),
        };
    }

    Rect ISvgVisual.Bounds => Data?.Bounds ?? Rect.Empty;

    Rect ISvgVisual.RenderBounds => RenderGeometry?.Bounds ?? Rect.Empty;

    void ISvgVisual.Render(DrawingContext dc)
    {
        if (RenderGeometry == null)
        {
            return;
        }

        dc.RenderWithOpacity(Opacity, () =>
        {
            dc.DrawGeometry(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), RenderGeometry);
        });
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
}
