using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.FillRule), SvgTypes.FillRule, SvgDefaultValues.FillRuleNonZero, false)]
public class FillRuleSetter : AbstractEnumSetter<FillRule>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IFillRuleSetter setter)
        {
            return;
        }

        setter.FillRule = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToFillRule();
    }
}
