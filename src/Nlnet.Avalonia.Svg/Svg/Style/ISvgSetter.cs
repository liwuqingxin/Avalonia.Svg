namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg style setter.
/// </summary>
public interface ISvgSetter
{
    /// <summary>
    /// Set value to <see cref="tag"/>.
    /// </summary>
    /// <param name="tag"></param>
    public void Set(ISvgTag tag);

    /// <summary>
    /// Initialize value by value string.
    /// </summary>
    /// <param name="setterValue"></param>
    void InitializeValue(string setterValue);

    /// <summary>
    /// Initialize value by deferred value string.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="deferredSetterValue"></param>
    public void InitializeDeferredValue(ISvgContext context, string deferredSetterValue);

    /// <summary>
    /// The value can not be parsed immediately. Add deferred value string and parse it later.
    /// </summary>
    /// <param name="valueString"></param>
    void AddDeferredValueString(string valueString);

    /// <summary>
    /// Apply the deferred value string if it exists.
    /// </summary>
    void ApplyDeferredValueString(ISvgContext context);
}
