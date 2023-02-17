using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.Data), typeof(Geometry), SvgDefaultValues.Null)]
public class DataSetter : AbstractGeometrySetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IDataSetter setter)
        {
            return;
        }

        setter.Data = Value;
    }
}
