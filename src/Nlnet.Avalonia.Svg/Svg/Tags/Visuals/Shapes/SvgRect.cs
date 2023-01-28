using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Factory is <see cref="SvgRectFactory"/>.
/// </summary>
[TagFactoryGenerator(nameof(SvgTags.rect))]
public class SvgRect : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    IIdSetter,
    IXSetter,
    IYSetter,
    IWidthSetter,
    IHeightSetter
{
    public double? X      { get; set; }
    public double? Y      { get; set; }
    public string? Id     { get; set; }
    public double? Width  { get; set; }
    public double? Height { get; set; }

    public SvgRect()
    {
        this.TryAddApplier(new ClassApplier());
        this.TryAddApplier(new DeferredPropertiesApplier());
    }

    public override void OnPropertiesFetched()
    {
        OriginalGeometry = new RectangleGeometry(new Rect(X ?? 0, Y ?? 0, Width ?? 0, Height ?? 0));
    }
}