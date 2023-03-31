using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(RefX), SvgDefaultValues.RefXDefault, true)]
public class RefXSetter : AbstractDeferredSetter
{
    private RefX? _value;

    public override void Set(ISvgTag tag)
    {
        if (tag is not IRefXSetter setter)
        {
            return;
        }

        setter.RefX = _value;
    }

    public override void InitializeValue(string setterValue)
    {
        _value = setterValue.ToRefX();
    }
}
