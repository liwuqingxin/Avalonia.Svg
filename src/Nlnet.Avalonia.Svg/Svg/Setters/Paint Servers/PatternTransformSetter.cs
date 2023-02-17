using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(Transform), SvgDefaultValues.Null)]
public class PatternTransformSetter : AbstractTransformSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IPatternTransformSetter setter)
        {
            return;
        }

        setter.PatternTransform = Value;
    }
}
