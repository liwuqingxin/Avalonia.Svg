namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Factory for <see cref="ISvgSetter"/>
/// </summary>
public interface ISvgSetterFactory
{
    /// <summary>
    /// Create a style setter instance.
    /// </summary>
    /// <returns></returns>
    public ISvgSetter CreateSetter();
}
