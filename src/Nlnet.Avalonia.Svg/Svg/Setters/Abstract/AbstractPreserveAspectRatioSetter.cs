namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="PreserveAspectRatio"/>.
/// </summary>
public abstract class AbstractPreserveAspectRatioSetter : AbstractDeferredSetter
{
    protected PreserveAspectRatio? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToPreserveAspectRatio();
    }
}
