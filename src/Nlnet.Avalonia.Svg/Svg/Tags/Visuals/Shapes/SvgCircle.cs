using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.circle))]
public class SvgCircle : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    ICXSetter,
    ICYSetter,
    IRSetter
{
    public double? CX { get; set; }

    public double? CY { get; set; }

    public double? R { get; set; }

    public override void OnPropertiesFetched()
    {
        if (CX != null && CY != null && R != null)
        {
            OriginalGeometry = new EllipseGeometry(new Rect(CX.Value - R.Value, CY.Value - R.Value, R.Value * 2, R.Value * 2));
        }
    }
}