using System.Xml;
using Avalonia.Media;

namespace Avalonia.Svg.Setters;

public interface IFillSetter : IDeferredAdder
{
    public IBrush? Fill { get; set; }

    public void Parse(XmlAttributeCollection attrs)
    {
        this.ParseOrDefer<IFillSetter, IBrush?>(attrs, SvgProperties.Fill, Parsers.TryToBrush, (setter, value) => setter.Fill = value);
    }
}

[Name(SvgProperties.Fill)] public class FillSetterFactory : AbstractSetterFactory<FillSetter> { }

public class FillSetter : AbstractBrushSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IFillSetter setter)
        {
            return;
        }

        setter.Fill = Brush;
    }
}
