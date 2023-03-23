using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    IDataSetter
{
    public Geometry? Data { get; set; }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        return Data;
    }
}
