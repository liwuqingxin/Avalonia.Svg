using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(double), SvgDefaultValues.Four, false)]
public class StrokeMiterLimitSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeMiterLimitSetter setter)
        {
            return;
        }

        setter.StrokeMiterLimit = Value;
    }
}
