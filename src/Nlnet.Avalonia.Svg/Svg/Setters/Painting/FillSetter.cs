using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(IBrush), SvgDefaultValues.BrushBlack)]
public class FillSetter : AbstractBrushSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IFillSetter setter)
        {
            return;
        }

        setter.Fill = Value;
    }
}
