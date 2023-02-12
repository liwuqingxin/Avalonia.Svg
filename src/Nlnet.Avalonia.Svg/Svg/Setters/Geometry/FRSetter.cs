using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.FR), SvgTypes.Double, SvgDefaultValues.Half, false)]
public class FRSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IFRSetter setter)
        {
            return;
        }

        setter.FR = Value;
    }
}
