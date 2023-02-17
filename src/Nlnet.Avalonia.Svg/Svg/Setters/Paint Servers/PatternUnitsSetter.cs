using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(SvgUnit), SvgDefaultValues.SvgUnitObjectBoundingBox, false)]
public class PatternUnitsSetter : AbstractEnumSetter<SvgUnit>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IPatternUnitsSetter setter)
        {
            return;
        }

        setter.PatternUnits = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToSvgUnit();
    }
}