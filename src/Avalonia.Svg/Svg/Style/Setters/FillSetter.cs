using Avalonia.Svg.CompileGenerator;

namespace Avalonia.Svg.Setters;

[SetterGenerator(nameof(SvgProperties.Fill), SvgTypes.Brush)]
public class FillSetter : AbstractBrushSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IFillSetter setter)
        {
            return;
        }

        setter.Fill = Brush;
    }
}
