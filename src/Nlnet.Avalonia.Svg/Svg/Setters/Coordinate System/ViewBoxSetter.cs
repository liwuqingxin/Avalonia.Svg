using Nlnet.Avalonia.Svg.CompileGenerator;
using System;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(ViewBox), SvgDefaultValues.Null)]
public class ViewBoxSetter : AbstractViewBoxSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IViewBoxSetter setter)
        {
            return;
        }

        setter.ViewBox = Value;
    }
}
