using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(SvgMarkerUnits), SvgDefaultValues.SvgMarkerUnitsStrokeWidth, false)]
public class MarkerUnitsSetter : AbstractEnumSetter<SvgMarkerUnits>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IMarkerUnitsSetter setter)
        {
            return;
        }

        setter.MarkerUnits = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToSvgMarkerUnits();
    }
}