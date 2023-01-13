using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.Points), SvgTypes.PointList, false, ParserMethodName = nameof(SvgTypes.PointList))]
public class PointsSetter : AbstractDoubleSetter
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