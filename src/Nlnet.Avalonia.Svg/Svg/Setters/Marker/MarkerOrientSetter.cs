using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(SvgMarkerOrient), SvgDefaultValues.SvgMarkerOrientAuto, false)]
public class MarkerOrientSetter : AbstractEnumSetter<SvgMarkerOrient>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IMarkerOrientSetter setter)
        {
            return;
        }

        setter.MarkerOrient = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToSvgMarkerOrient();
    }
}
