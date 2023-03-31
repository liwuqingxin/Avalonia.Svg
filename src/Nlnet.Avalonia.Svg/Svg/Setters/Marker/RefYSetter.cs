using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(RefY), SvgDefaultValues.RefYDefault, true)]
public class RefYSetter : AbstractDeferredSetter
{
    private RefY? _value;

    public override void Set(ISvgTag tag)
    {
        if (tag is not IRefYSetter setter)
        {
            return;
        }

        setter.RefY = _value;
    }

    public override void InitializeValue(string setterValue)
    {
        _value = setterValue.ToRefY();
    }
}