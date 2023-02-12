using System;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.Class), typeof(String), SvgDefaultValues.Null, ParserMethodName = "String")]
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
