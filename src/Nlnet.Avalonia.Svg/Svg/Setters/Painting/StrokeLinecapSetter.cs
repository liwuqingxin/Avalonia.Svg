using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.StrokeLineCap), SvgTypes.PenLineCap, SvgDefaultValues.PenLineCapNonZero, false)]
public class StrokeLineCapSetter : AbstractEnumSetter<PenLineCap>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeLineCapSetter setter)
        {
            return;
        }

        setter.StrokeLineCap = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToPenLineCap();
    }
}
