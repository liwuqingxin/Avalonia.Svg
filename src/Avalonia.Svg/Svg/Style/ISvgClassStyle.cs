using System.Collections.Generic;

namespace Avalonia.Svg;

/// <summary>
/// Svg class style definition.
/// </summary>
public interface ISvgClassStyle
{
    /// <summary>
    /// Class of this style.
    /// </summary>
    public string Class { get; set; }

    /// <summary>
    /// Setters of this style.
    /// </summary>
    public List<ISvgStyleSetter> Setters { get; set; }

    /// <summary>
    /// Apply this style to <see cref="tag"/>.
    /// </summary>
    /// <param name="tag"></param>
    public void ApplyTo(ISvgTag tag);
}