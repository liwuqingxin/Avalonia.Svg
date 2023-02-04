using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.Stroke), SvgTypes.Brush, SvgDefaultValues.BrushTransparent)]
public class StrokeSetter : AbstractBrushSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IStrokeSetter setter)
        {
            return;
        }

        setter.Stroke = Value;
    }
}
