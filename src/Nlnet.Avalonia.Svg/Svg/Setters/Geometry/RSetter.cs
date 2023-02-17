﻿using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.R), typeof(double), SvgDefaultValues.Zero, false)]
public class RSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IRSetter setter)
        {
            return;
        }

        setter.R = Value;
    }
}