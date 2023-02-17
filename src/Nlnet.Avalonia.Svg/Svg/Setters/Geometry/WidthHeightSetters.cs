﻿using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.Width), typeof(double), SvgDefaultValues.Zero, false)]
public class WidthSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IWidthSetter setter)
        {
            return;
        }

        setter.Width = Value;
    }
}

[SetterGenerator(nameof(SvgProperties.Height), typeof(double), SvgDefaultValues.Zero, false)]
public class HeightSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IHeightSetter setter)
        {
            return;
        }

        setter.Height = Value;
    }
}