using System;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(string), SvgDefaultValues.Null)]
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
