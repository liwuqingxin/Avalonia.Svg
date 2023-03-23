using Nlnet.Avalonia.Svg.CompileGenerator;
using System;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(ISvgStyle), SvgDefaultValues.Null)]
public class StyleSetter : AbstractStyleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStyleSetter setter)
        {
            return;
        }

        setter.Style = Value;
    }
}
