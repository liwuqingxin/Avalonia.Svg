using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polyline))]
public class SvgPolyline : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    IClassSetter,
    IPointsSetter
{
    public string? Class { get; set; }

    public PointList? Points { get; set; }

    public SvgPolyline()
    {
        this.TryAddApplier(new ClassApplier());
        this.TryAddApplier(new DeferredPropertiesApplier());
    }

    public override void OnPropertiesFetched()
    {
        if (Points != null)
        {
            OriginalGeometry = new PolylineGeometry(Points, false);
        }
    }
}