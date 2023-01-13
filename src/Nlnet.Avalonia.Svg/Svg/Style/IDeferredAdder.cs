namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Provide the ability to add a deferred property to parse value.
/// </summary>
public interface IDeferredAdder
{
    /// <summary>
    /// Add a deferred property to parse value.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="valueString"></param>
    public void AddDeferred(string property, string valueString);
}
