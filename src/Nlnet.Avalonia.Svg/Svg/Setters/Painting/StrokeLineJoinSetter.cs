using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.StrokeLineJoin), SvgTypes.PenLineJoin, SvgDefaultValues.PenLineJoinNonZero, false)]
public class StrokeLineJoinSetter : AbstractEnumSetter<PenLineJoin>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeLineJoinSetter setter)
        {
            return;
        }

        setter.StrokeLineJoin = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToPenLineJoin();
    }
}
