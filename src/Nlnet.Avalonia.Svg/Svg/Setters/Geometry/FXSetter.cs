using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.FX), SvgTypes.Double, SvgDefaultValues.Half, false)]
public class FXSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IFXSetter setter)
        {
            return;
        }

        setter.FX = Value;
    }
}
