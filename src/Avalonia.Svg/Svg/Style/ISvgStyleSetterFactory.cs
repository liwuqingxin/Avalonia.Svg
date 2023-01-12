namespace Avalonia.Svg;

/// <summary>
/// Factory for <see cref="ISvgStyleSetter"/>
/// </summary>
public interface ISvgStyleSetterFactory
{
    /// <summary>
    /// Create a style setter instance.
    /// </summary>
    /// <returns></returns>
    public ISvgStyleSetter CreateSetter();
}
