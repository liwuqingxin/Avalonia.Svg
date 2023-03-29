using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(string), SvgDefaultValues.Null, true)]
public class MarkerStartSetter : AbstractStringSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IMarkerStartSetter setter)
        {
            return;
        }

        setter.MarkerStart = Value;
    }
}
