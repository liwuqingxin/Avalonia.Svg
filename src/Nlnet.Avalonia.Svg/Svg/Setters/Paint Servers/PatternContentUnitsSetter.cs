﻿using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(SvgUnit), SvgDefaultValues.SvgUnitObjectBoundingBox, false)]
public class PatternContentUnitsSetter : AbstractEnumSetter<SvgUnit>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IPatternContentUnitsSetter setter)
        {
            return;
        }

        setter.PatternContentUnits = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToSvgUnit();
    }
}
