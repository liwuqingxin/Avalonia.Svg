using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(double), SvgDefaultValues.One, false)]
public class StrokeOpacitySetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeOpacitySetter setter)
        {
            return;
        }

        setter.StrokeOpacity = Value;
    }
}