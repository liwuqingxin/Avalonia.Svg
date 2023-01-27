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
    public IReadOnlyDictionary<string, ISvgClassStyle> Styles { get; }

    /// <summary>
    /// All brushes.
    /// </summary>
    public IReadOnlyDictionary<string, IBrush> Brushes { get; }

    /// <summary>
    /// All renderable elements.
    /// </summary>
    public IReadOnlyList<ISvgRenderable> Renderables { get; }
}