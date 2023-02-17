using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.StrokeDashArray), typeof(DoubleList), SvgDefaultValues.Null)]
public class StrokeDashArraySetter : AbstractDoubleListSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeDashArraySetter setter)
        {
            return;
        }

        setter.StrokeDashArray = Value;
    }
}
