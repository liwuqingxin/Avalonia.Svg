using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(string), SvgDefaultValues.Null, true)]
public class MaskSetter : AbstractStringSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IMaskSetter setter)
        {
            return;
        }

        setter.Mask = Value;
    }
}
