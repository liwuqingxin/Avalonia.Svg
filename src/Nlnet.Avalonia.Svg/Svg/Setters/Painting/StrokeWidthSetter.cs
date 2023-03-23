using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(double), SvgDefaultValues.One, false)]
public class StrokeWidthSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeWidthSetter setter)
        {
            return;
        }

        setter.StrokeWidth = Value;
    }
}