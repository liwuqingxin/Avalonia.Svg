using System.Xml;

namespace Avalonia.Svg.Setters;

public interface IStrokeWidthSetter : IDeferredAdder
{
    public double? StrokeWidth { get; set; }

    public void Parse(XmlAttributeCollection attrs)
    {
        this.ParseOrDefer<IStrokeWidthSetter, double>(attrs, SvgProperties.StrokeWidth, Parsers.TryToDouble, (setter, value) => setter.StrokeWidth = value);
    }
}

[Name(SvgProperties.StrokeWidth)] public class StrokeWidthSetterFactory : AbstractSetterFactory<StrokeWidthSetter> { }

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
