using Avalonia.Svg.CompileGenerator;

namespace Avalonia.Svg.Setters;

[SetterGenerator(nameof(SvgProperties.Data), SvgTypes.Geometry)]
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
