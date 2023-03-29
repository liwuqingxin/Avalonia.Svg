using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(RefY), SvgDefaultValues.Zero, false)]
public class RefYSetter : AbstractEnumSetter<RefY>
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IRefYSetter setter)
        {
            return;
        }

        setter.RefY = Value;
    }

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToRefY();
    }
}