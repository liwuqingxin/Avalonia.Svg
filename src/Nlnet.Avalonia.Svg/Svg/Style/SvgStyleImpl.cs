using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Default implementation of <see cref="ISvgStyle"/>.
/// </summary>
public class SvgStyleImpl : ISvgStyle
{
    public SvgStyleImpl(string @class, IEnumerable<ISvgSetter> setters)
    {
        Class   = @class;
        Setters = setters;
    }

    public string Class { get; set; }

    public IEnumerable<ISvgSetter> Setters { get; set; }

    void ISvgStyle.ApplyTo(ISvgTag tag)
    {
        foreach (var setter in Setters)
        {
            setter.Set(tag);
        }
    }
}
