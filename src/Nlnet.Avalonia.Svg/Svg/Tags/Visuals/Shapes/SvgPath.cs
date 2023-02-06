using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    IDataSetter
{
    public Geometry? Data { get; set; }

    public override void OnPropertiesFetched()
    {
        if (Data != null)
        {
            OriginalGeometry = Data;
        }
    }
}
