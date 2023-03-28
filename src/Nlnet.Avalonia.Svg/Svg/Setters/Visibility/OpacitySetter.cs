using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(string), SvgDefaultValues.Null, true)]
public class ClipPathSetter : AbstractStringSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IClipPathSetter setter)
        {
            return;
        }

        setter.ClipPath = Value;
    }
}
