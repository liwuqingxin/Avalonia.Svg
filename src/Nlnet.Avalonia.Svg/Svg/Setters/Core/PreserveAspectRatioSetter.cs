using Nlnet.Avalonia.Svg.CompileGenerator;
using System;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(PreserveAspectRatio), SvgDefaultValues.Null)]
public class PreserveAspectRatioSetter : AbstractPreserveAspectRatioSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IPreserveAspectRatioSetter setter)
        {
            return;
        }

        setter.PreserveAspectRatio = Value;
    }
}
