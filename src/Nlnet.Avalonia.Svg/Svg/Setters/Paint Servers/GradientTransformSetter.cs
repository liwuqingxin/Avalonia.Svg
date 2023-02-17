using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.GradientTransform), typeof(Transform), SvgDefaultValues.Null)]
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
