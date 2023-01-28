using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Default implementation of <see cref="ISvgStyle"/>.
/// </summary>
public class SvgStyleInstance : ISvgStyle
{
    public SvgStyleInstance(string @class, List<ISvgSetter> setters)
    {
        Class   = @class;
        Setters = setters;
    }

    public string Class { get; set; }

    public List<ISvgSetter> Setters { get; set; }

    void ISvgStyle.ApplyTo(ISvgTag tag)
    {
        foreach (var setter in Setters)
        {
            setter.Set(tag);
        }
    }
}
