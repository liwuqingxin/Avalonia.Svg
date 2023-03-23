using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(SvgUnit), SvgDefaultValues.SvgUnitObjectBoundingBox, false)]
public class GradientUnitsSetter : AbstractEnumSetter<SvgUnit>
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
        Value = setterValue.ToSvgUnit();
    }
}
