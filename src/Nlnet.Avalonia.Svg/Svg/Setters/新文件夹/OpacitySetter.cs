using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(double), SvgDefaultValues.Zero, false)]
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
