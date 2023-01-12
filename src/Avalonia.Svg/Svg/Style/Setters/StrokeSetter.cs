using System.Xml;
using Avalonia.Media;

namespace Avalonia.Svg.Setters;

public interface IStrokeSetter : IDeferredAdder
{
    public IBrush? Stroke { get; set; }

    public void Parse(XmlAttributeCollection attrs)
    {
        this.ParseOrDefer<IStrokeSetter, IBrush?>(attrs, SvgProperties.Stroke, Parsers.TryToBrush, (setter, value) => setter.Stroke = value);
    }
}

[Name(SvgProperties.Stroke)] public class StrokeSetterFactory : AbstractSetterFactory<StrokeSetter> { }

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
