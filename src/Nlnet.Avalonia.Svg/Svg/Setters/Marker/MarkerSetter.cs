using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(string), SvgDefaultValues.Null, true)]
public class MarkerSetter : AbstractStringSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IMarkerSetter setter)
        {
            return;
        }

        setter.Marker = Value;
    }
}