using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="FillRule"/>.
/// </summary>
public abstract class AbstractFillRuleSetter : AbstractDeferredSetter
{
    protected FillRule? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToFillRule();
    }
}
