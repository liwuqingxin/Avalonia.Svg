using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(double), SvgDefaultValues.Zero, false)]
public class StrokeDashOffsetSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeDashOffsetSetter setter)
        {
            return;
        }

        setter.StrokeDashOffset = Value;
    }
}
