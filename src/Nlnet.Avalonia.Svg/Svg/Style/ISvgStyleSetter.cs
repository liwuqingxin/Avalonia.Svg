namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg style setter.
/// </summary>
public interface ISvgStyleSetter
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
    /// <param name="collector"></param>
    /// <param name="deferredSetterValue"></param>
    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue);
}
