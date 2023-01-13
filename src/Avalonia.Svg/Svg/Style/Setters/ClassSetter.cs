using Avalonia.Svg.CompileGenerator;

namespace Avalonia.Svg.Setters;

[SetterGenerator(nameof(SvgProperties.Class), SvgTypes.String)]
public class ClassSetter : AbstractStringSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IClassSetter setter)
        {
            return;
        }

        setter.Class = Value;
    }
}
