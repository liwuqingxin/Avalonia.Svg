using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.Points), SvgTypes.PointList, false)]
public class PointsSetter : AbstractPointsSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IPointsSetter setter)
        {
            return;
        }

        setter.Points = Value;
    }
}