using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(PointList), SvgDefaultValues.Null, false)]
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