using System;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="double"/>.
/// </summary>
public abstract class AbstractDoubleSetter : AbstractDeferredSetter
{
    protected double? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToDouble();
    }
}
