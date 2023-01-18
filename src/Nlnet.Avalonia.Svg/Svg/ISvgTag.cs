using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Standard svg tag definition.
/// </summary>
public interface ISvgTag
{
    /// <summary>
    /// Properties deferred to set value.
    /// </summary>
    public IReadOnlyDictionary<string, string>? DeferredProperties { get; }

    /// <summary>
    /// Parent of the svg tag.
    /// </summary>
    public ISvgTag? Parent { get; set; }

    /// <summary>
    /// Children of this tag.
    /// </summary>
    public List<ISvgTag>? Children { get; set; }

    /// <summary>
    /// Get the standard tag name. It is defined by svg format.
    /// </summary>
    /// <returns></returns>
    public string GetTagName();

    /// <summary>
    /// Apply resources to this tag including brushes, styles, etc...
    /// </summary>
    /// <param name="collector"></param>
    public void ApplyResources(ISvgResourceCollector collector);

    /// <summary>
    /// This method will be called after properties of svg tag being fetched.
    /// </summary>
    public void OnPropertiesFetched();
}
