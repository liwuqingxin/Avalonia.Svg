using System;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="PointList"/>.
/// </summary>
public abstract class AbstractPointsSetter : AbstractDeferredSetter
{
    protected PointList? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToPointList();
    }
}
