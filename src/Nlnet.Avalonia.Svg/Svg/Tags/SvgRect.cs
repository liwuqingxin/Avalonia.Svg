using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;
using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.rect))]
public class SvgRect : SvgTagBase,
    IIdSetter,
    IXSetter,
    IYSetter,
    IWidthSetter,
    IHeightSetter,
    ISvgVisual
{
    private Rect _renderBounds;

    public double? X           { get; set; }
    public double? Y           { get; set; }
    public string? Id          { get; set; }
    public double? Width       { get; set; }
    public double? Height      { get; set; }
    public IBrush? Fill        { get; set; }
    public IBrush? Stroke      { get; set; }
    public double? StrokeWidth { get; set; }
    public double? Opacity     { get; set; }

    public SvgRect()
    {
        ResourceAppliers = new List<ISvgResourceApplier>()
        {
            new ClassApplier(),
            new DeferredPropertiesApplier(),
        };
    }

    Rect ISvgVisual.Bounds => new(X ?? 0, Y ?? 0, Width ?? 0, Height ?? 0);

    Rect ISvgVisual.RenderBounds => _renderBounds;

    void ISvgVisual.Render(DrawingContext dc)
    {
        if (Width == null || Height == null || (Width == 0 && Height == 0))
        {
            return;
        }

        dc.RenderWithOpacity(Opacity, () =>
        {
            dc.DrawRectangle(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), _renderBounds);
        });
    }

    void ISvgVisual.ApplyTransform(Transform transform)
    {
        _renderBounds = ((ISvgVisual)this).Bounds.TransformToAABB(transform.Value);
    }
}