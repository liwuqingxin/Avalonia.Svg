using Avalonia.Media;
using Avalonia;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.ellipse))]
public class SvgEllipse : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    IRXSetter,
    IRYSetter,
    ICXSetter,
    ICYSetter
{
    public double? RX { get; set; }

    public double? RY { get; set; }

    public double? CX { get; set; }

    public double? CY { get; set; }

    public override void OnPropertiesFetched()
    {
        if (CX != null && CY != null && RX != null && RY != null)
        {
            OriginalGeometry = new EllipseGeometry(new Rect(CX.Value - RX.Value, CY.Value - RY.Value, RX.Value * 2, RY.Value * 2));
        }
    }
}