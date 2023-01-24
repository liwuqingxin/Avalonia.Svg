using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;
using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.rect))]
public class SvgRect : SvgVisualBase, IShape, IGraphic,
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
        ResourceAppliers = new List<ISvgResourceApplier>()
        {
            new ClassApplier(),
            new DeferredPropertiesApplier(),
        };
    }

    public override void OnPropertiesFetched()
    {
        OriginalGeometry = new RectangleGeometry(new Rect(X ?? 0, Y ?? 0, Width ?? 0, Height ?? 0));
    }
}