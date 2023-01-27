namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter factory.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AbstractSetterFactory<T> : ISvgStyleSetterFactory where T : ISvgStyleSetter, new()
{
    public ISvgStyleSetter CreateSetter()
    {
        return new T();
    }
}
