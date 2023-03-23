namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="ViewBox"/>.
/// </summary>
public abstract class AbstractViewBoxSetter : AbstractDeferredSetter
{
    protected ViewBox? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToViewBox();
    }
}
