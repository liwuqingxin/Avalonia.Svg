using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Standard svg tag definition.
/// </summary>
public interface ISvgTag : 
    IClassSetter, 
    IIdSetter,
    IStyleSetter
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
    /// Check if the tag is a definition.
    /// </summary>
    public bool IsDef { get; set; }

    /// <summary>
    /// Get the standard tag name. It is defined by svg format.
    /// </summary>
    /// <returns></returns>
    public string GetTagName();

    /// <summary>
    /// Apply context to this tag including brushes, styles, etc...
    /// </summary>
    /// <param name="context"></param>
    public void ApplyContext(ISvgContext context);

    /// <summary>
    /// This method will be called after properties of svg tag being fetched.
    /// </summary>
    public void OnPropertiesFetched();

    /// <summary>
    /// Get property value even if the property can be inherit from ancestor.
    /// </summary>
    /// <typeparam name="TPropertyOwner"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public TValue? GetPropertyValue<TPropertyOwner, TValue>()
        where TPropertyOwner : class, ISvgProperty<TPropertyOwner>
        where TValue : class;

    /// <summary>
    /// Get property value even if the property can be inherit from ancestor.
    /// </summary>
    /// <typeparam name="TPropertyOwner"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public TValue GetPropertyStructValue<TPropertyOwner, TValue>()
        where TPropertyOwner : class, ISvgProperty<TPropertyOwner>
        where TValue : struct;
}
