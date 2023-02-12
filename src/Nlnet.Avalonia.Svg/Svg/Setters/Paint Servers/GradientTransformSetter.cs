using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.GradientTransform), SvgTypes.Transform, SvgDefaultValues.Null)]
public class GradientTransformSetter : AbstractTransformSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IGradientTransformSetter setter)
        {
            return;
        }

        setter.GradientTransform = Value;
    }
}
