namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter factory.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AbstractSetterFactory<T> : ISvgSetterFactory where T : ISvgSetter, new()
{
    public ISvgSetter CreateSetter()
    {
        return new T();
    }
}
