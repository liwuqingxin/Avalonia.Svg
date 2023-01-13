using Avalonia.Svg.CompileGenerator;

namespace Avalonia.Svg.Setters;

[SetterGenerator(nameof(SvgProperties.Opacity), SvgTypes.Double, false)]
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
