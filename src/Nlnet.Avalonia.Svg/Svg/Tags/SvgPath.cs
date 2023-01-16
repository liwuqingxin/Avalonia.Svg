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
    private Geometry? _renderGeometry;

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

    Rect ISvgVisual.RenderBounds => _renderGeometry?.Bounds ?? Rect.Empty;

    void ISvgVisual.Render(DrawingContext dc)
    {
        if (_renderGeometry == null)
        {
            return;
        }

        dc.RenderWithOpacity(Opacity, () =>
        {
            dc.DrawGeometry(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), _renderGeometry);
        });
    }

    void ISvgVisual.ApplyTransform(Transform transform)
    {
        if (Data == null)
        {
            return;
        }

        _renderGeometry = Data.Clone();
        _renderGeometry.Transform = transform;
    }
}
