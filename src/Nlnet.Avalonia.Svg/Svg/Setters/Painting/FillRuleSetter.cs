using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.FillRule), SvgTypes.FillRule, SvgDefaultValues.FillRuleNonZero)]
public class FillRuleSetter : AbstractFillRuleSetter
{
    public FillRuleSetter()
    {
        
    }

    public override void Set(ISvgTag tag)
    {
        if (tag is not IFillRuleSetter setter)
        {
            return;
        }

        setter.FillRule = Value;
    }
}
