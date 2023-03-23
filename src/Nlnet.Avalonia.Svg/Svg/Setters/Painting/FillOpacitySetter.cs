using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(double), SvgDefaultValues.One, false)]
public class FillOpacitySetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IFillOpacitySetter setter)
        {
            return;
        }

        setter.FillOpacity = Value;
    }
}
