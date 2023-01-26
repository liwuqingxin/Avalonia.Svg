using System;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="Transform"/>.
/// </summary>
public abstract class AbstractTransformSetter : AbstractDeferredSetter
{
    protected Transform? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToTransform();
    }
}