using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.FillOpacity), typeof(double), SvgDefaultValues.One, false)]
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
