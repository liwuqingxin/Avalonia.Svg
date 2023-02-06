using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Factory is <see cref="SvgRectFactory"/>.
/// </summary>
[TagFactoryGenerator(nameof(SvgTags.rect))]
public class SvgRect : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
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

    public override void OnPropertiesFetched()
    {
        if (X != null && Y != null && Width != null && Height != null)
        {
            OriginalGeometry = new RectangleGeometry(new Rect(X.Value, Y.Value, Width.Value, Height.Value));
        }
    }
}