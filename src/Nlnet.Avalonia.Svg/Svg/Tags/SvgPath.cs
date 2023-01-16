using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : SvgVisualBase, 
    IClassSetter, 
    IDataSetter,
    ISvgVisual
{
    private Geometry? _renderGeometry;

    public string? Class { get; set; }
    public Geometry? Data { get; set; }

    public SvgPath()
    {
        ResourceAppliers = new List<ISvgResourceApplier>()
        {
            new ClassApplier(),
            new DeferredPropertiesApplier(),
        };
    }

    public override Rect Bounds => Data?.Bounds ?? Rect.Empty;

    public override Rect RenderBounds => _renderGeometry?.Bounds ?? Rect.Empty;

    public override void Render(DrawingContext dc)
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

    protected override void ApplyTransformCore(Transform transform)
    {
        if (Data == null)
        {
            return;
        }

        _renderGeometry = Data.Clone();
        _renderGeometry.Transform = transform;
    }
}
