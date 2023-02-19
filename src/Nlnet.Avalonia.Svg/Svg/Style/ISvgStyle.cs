using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg class style definition.
/// </summary>
public interface ISvgStyle
{
    /// <summary>
    /// Class of this style.
    /// </summary>
    public string Class { get; set; }

    /// <summary>
    /// Setters of this style.
    /// </summary>
    public IEnumerable<ISvgSetter> Setters { get; set; }

    /// <summary>
    /// Apply this style to <see cref="tag"/>.
    /// </summary>
    /// <param name="tag"></param>
    public void ApplyTo(ISvgTag tag);
}