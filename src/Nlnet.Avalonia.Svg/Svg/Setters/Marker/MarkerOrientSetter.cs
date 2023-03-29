using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(SvgMarkerOrient), SvgDefaultValues.SvgMarkerOrientAuto, true)]
public class MarkerOrientSetter : AbstractDeferredSetter
{
    private SvgMarkerOrient? _value;

    public override void Set(ISvgTag tag)
    {
        if (tag is not IMarkerOrientSetter setter)
        {
            return;
        }

        setter.MarkerOrient = _value;
    }

    public override void InitializeValue(string setterValue)
    {
        _value = setterValue.ToSvgMarkerOrient();
    }
}
