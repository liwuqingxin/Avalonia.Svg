using System.Xml;

namespace Avalonia.Svg.Setters;

public interface IOpacitySetter : IDeferredAdder
{
    public double? Opacity { get; set; }

    public void Parse(XmlAttributeCollection attrs)
    {
        this.ParseOrDefer<IOpacitySetter, double>(attrs, SvgProperties.Opacity, Parsers.TryToDouble, (setter, value) => setter.Opacity = value);
    }
}

[Name(SvgProperties.Opacity)] public class OpacitySetterFactory : AbstractSetterFactory<OpacitySetter> { }

public class OpacitySetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IOpacitySetter setter)
        {
            return;
        }

        setter.Opacity = Value;
    }
}
