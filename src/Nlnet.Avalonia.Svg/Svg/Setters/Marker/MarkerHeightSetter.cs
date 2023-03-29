using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(double), SvgDefaultValues.Three, false)]
public class MarkerHeightSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IMarkerHeightSetter setter)
        {
            return;
        }

        setter.MarkerHeight = Value;
    }
}