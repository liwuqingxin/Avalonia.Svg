using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(double), SvgDefaultValues.Three, false)]
public class MarkerWidthSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IMarkerWidthSetter setter)
        {
            return;
        }

        setter.MarkerWidth = Value;
    }
}