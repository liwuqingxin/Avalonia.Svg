using Avalonia.Svg.CompileGenerator;

namespace Avalonia.Svg.Setters;

[SetterGenerator(nameof(SvgProperties.Stroke), SvgTypes.Brush)]
public class StrokeSetter : AbstractBrushSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeSetter setter)
        {
            return;
        }

        setter.Stroke = Brush;
    }
}
