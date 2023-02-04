using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.StrokeMiterLimit), SvgTypes.Double, SvgDefaultValues.Four, false)]
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
