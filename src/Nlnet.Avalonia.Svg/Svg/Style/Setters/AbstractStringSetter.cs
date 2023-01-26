using System;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="string"/>.
/// </summary>
public abstract class AbstractStringSetter : AbstractDeferredSetter
{
    protected string? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue;
    }
}
