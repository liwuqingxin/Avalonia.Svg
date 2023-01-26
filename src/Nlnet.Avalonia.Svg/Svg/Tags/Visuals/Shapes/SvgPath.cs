using System.Collections.Generic;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : SvgVisualBase, IShape, IGraphic, IRenderable,
    IClassSetter, 
    IDataSetter
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
}
