using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(RefX), SvgDefaultValues.Zero, false)]
public class RefXSetter : AbstractEnumSetter<RefX>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IRefXSetter setter)
        {
            return;
        }

        setter.RefX = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToRefX();
    }
}
