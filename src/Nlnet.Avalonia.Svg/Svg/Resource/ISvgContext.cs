using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg context that contains resources and...
/// </summary>
public interface ISvgContext
{
    /// <summary>
    /// Gets the value that indicates if should show diagnosis visuals.
    /// </summary>
    public bool ShowDiagnosis { get; }

    /// <summary>
    /// The size of container.
    /// </summary>
    public Size ContainerSize { get; }

    /// <summary>
    /// All styles.
    /// </summary>
    public IReadOnlyList<ISvgStyle> Styles { get; }

    /// <summary>
    /// All brushes.
    /// </summary>
    public IReadOnlyDictionary<string, IBrush> Brushes { get; }

    /// <summary>
    /// All clip paths.
    /// </summary>
    public IReadOnlyDictionary<string, SvgClipPath> ClipPaths { get; }

    /// <summary>
    /// All masks.
    /// </summary>
    public IReadOnlyDictionary<string, SvgMask> Masks { get; }

    /// <summary>
    /// ALl Markers.
    /// </summary>
    public IReadOnlyDictionary<string, SvgMarker> Markers { get; }

    /// <summary>
    /// All tags that have a id.
    /// </summary>
    public IReadOnlyDictionary<string, ISvgTag> IdTags { get; }

    /// <summary>
    /// All renderable elements.
    /// </summary>
    public IReadOnlyList<ISvgRenderable> Renderables { get; }
}