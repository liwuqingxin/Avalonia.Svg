using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.FY), typeof(double), SvgDefaultValues.Half, false)]
public class FYSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IFYSetter setter)
        {
            return;
        }

        setter.FY = Value;
    }
}
