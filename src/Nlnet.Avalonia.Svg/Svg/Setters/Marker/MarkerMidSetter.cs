using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(string), SvgDefaultValues.Null, true)]
public class MarkerMidSetter : AbstractStringSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IMarkerMidSetter setter)
        {
            return;
        }

        setter.MarkerMid = Value;
    }
}
