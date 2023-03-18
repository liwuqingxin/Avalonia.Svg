using System.Collections.Generic;
using System.Linq;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Default implementation of <see cref="ISvgStyle"/>.
/// </summary>
public class SvgStyleImpl : ISvgStyle
{
    private readonly IEnumerable<IStyleSelector>? _selectors;

    public SvgStyleImpl(IEnumerable<IStyleSelector>? selectors, IEnumerable<ISvgSetter> setters)
    {
        _selectors = selectors;
        Setters    = setters;
    }

    public IEnumerable<ISvgSetter> Setters { get; set; }

    void ISvgStyle.ApplyTo(ISvgTag tag)
    {
        foreach (var setter in Setters)
        {
            setter.Set(tag);
        }
    }

    public bool Match(ISvgTag tag)
    {
        if (_selectors == null)
        {
            return false;
        }

        return _selectors.Any(s => s.Match(tag));
    }
}
