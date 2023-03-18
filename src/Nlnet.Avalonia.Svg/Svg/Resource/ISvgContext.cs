using System.Collections.Generic;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg context that contains resources and...
/// </summary>
public interface ISvgContext
{
    /// <summary>
    /// All styles.
    /// </summary>
    public IReadOnlyList<ISvgStyle> Styles { get; }

    /// <summary>
    /// All brushes.
    /// </summary>
    public IReadOnlyDictionary<string, LightBrush> Brushes { get; }

    /// <summary>
    /// All tags that have a id.
    /// </summary>
    public IReadOnlyDictionary<string, ISvgTag> IdTags { get; }

    /// <summary>
    /// All renderable elements.
    /// </summary>
    public IReadOnlyList<ISvgRenderable> Renderables { get; }
}