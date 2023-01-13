using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Default implementation of <see cref="ISvgClassStyle"/>.
/// </summary>
public class SvgClassStyle : ISvgClassStyle
{
    public SvgClassStyle(string @class, List<ISvgStyleSetter> setters)
    {
        Class   = @class;
        Setters = setters;
    }

    public string Class { get; set; }

    public List<ISvgStyleSetter> Setters { get; set; }

    public void ApplyTo(ISvgTag tag)
    {
        foreach (var setter in Setters)
        {
            setter.Set(tag);
        }
    }
}
