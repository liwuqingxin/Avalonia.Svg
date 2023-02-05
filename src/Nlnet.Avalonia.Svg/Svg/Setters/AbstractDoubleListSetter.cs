using System;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="DoubleList"/>.
/// </summary>
public abstract class AbstractDoubleListSetter : AbstractDeferredSetter
{
    protected DoubleList? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToDoubleList();
    }
}
