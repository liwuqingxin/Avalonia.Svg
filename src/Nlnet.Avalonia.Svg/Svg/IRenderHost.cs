namespace Nlnet.Avalonia.Svg;

/// <summary>
/// It means it can render itself and it's children.
/// </summary>
public interface IRenderHost
{
    /// <summary>
    /// Indicates if this object renders itself and it's children by itself.
    /// </summary>
    public bool RenderBySelf { get; }
}
