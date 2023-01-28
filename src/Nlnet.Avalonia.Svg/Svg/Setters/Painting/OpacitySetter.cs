using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.Opacity), SvgTypes.Double, SvgDefaultValues.Zero, false)]
public class OpacitySetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IOpacitySetter setter)
        {
            return;
        }

        setter.Opacity = Value;
    }
}
