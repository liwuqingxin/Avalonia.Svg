using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Standard svg tag definition.
/// </summary>
public interface ISvgTag
{
    /// <summary>
    /// Get the standard tag name. It is defined by svg format.
    /// </summary>
    /// <returns></returns>
    public string GetTagName();

    /// <summary>
    /// Children of this tag.
    /// </summary>
    public List<ISvgTag>? Children { get; set; }

    /// <summary>
    /// Apply resources to this tag including brushes, styles, etc...
    /// </summary>
    /// <param name="collector"></param>
    public void ApplyResources(ISvgResourceCollector collector);
}
