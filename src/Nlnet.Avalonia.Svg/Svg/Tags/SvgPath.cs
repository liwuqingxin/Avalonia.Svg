using System.Collections.Generic;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : SvgVisualBase, 
    IClassSetter, 
    IDataSetter,
    ISvgVisual
{
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

    public override void OnPropertiesFetched()
    {
        OriginalGeometry = Data ?? new PolylineGeometry();
    }

    public override void Render(DrawingContext dc)
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
}
