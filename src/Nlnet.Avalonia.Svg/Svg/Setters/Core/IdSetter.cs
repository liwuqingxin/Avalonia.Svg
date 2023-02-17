using Nlnet.Avalonia.Svg.CompileGenerator;
using System;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(string), SvgDefaultValues.Null)]
public class IdSetter : AbstractStringSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IIdSetter setter)
        {
            return;
        }

        setter.Id = Value;
    }
}
