using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(GradientSpreadMethod), SvgDefaultValues.GradientSpreadMethodPad, false)]
public class GradientSpreadMethodSetter : AbstractEnumSetter<GradientSpreadMethod>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IGradientSpreadMethodSetter setter)
        {
            return;
        }

        setter.GradientSpreadMethod = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToGradientSpreadMethod();
    }
}
