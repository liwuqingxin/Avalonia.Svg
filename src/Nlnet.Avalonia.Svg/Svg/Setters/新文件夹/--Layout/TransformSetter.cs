using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.Transform), typeof(Transform), SvgDefaultValues.Null, false)]
public class TransformSetter : AbstractTransformSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not ITransformSetter setter)
        {
            return;
        }

        setter.Transform = Value;
    }
}