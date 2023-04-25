using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;
using Nlnet.Avalonia.Svg.Utils;

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

    protected override Geometry? OnCreateOriginalGeometry()
    {
        if (Width == null || Height == null)
        {
            return null;
        }

        var x = X ?? 0;
        var y = Y ?? 0;

        return new RectangleGeometry(new Rect(x, y, Width.Value, Height.Value));
    }

    //protected override ImmutableTransform? GetBrushTransform()
    //{
    //    var t1 = Transform?.Value ?? Matrix.Identity;
    //    var t2 = new Matrix(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, X ?? 0.0, Y ?? 0.0, 1.0);
    //    return new ImmutableTransform(t1 * t2);
    //}
}
