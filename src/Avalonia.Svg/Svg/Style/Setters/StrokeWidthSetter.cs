using Avalonia.Svg.CompileGenerator;

namespace Avalonia.Svg.Setters;

[SetterGenerator(nameof(SvgProperties.StrokeWidth), SvgTypes.Double, false)]
public class StrokeWidthSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeWidthSetter setter)
        {
            return;
        }

        setter.StrokeWidth = Value;
    }
}
