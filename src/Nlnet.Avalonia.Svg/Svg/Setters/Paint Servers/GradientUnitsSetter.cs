using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

public enum GradientUnit
{
    objectBoundingBox = 0,
    userSpaceOnUse = 1,
}

[SetterGenerator(nameof(SvgProperties.GradientUnits), SvgTypes.GradientUnit, SvgDefaultValues.GradientUnitObjectBoundingBox, false)]
public class GradientUnitsSetter : AbstractEnumSetter<GradientUnit>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IGradientUnitsSetter setter)
        {
            return;
        }

        setter.GradientUnits = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToGradientUnit();
    }
}
