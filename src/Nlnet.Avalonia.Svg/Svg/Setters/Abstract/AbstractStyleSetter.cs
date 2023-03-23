using System;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="ISvgStyle"/>.
/// </summary>
public abstract class AbstractStyleSetter : AbstractDeferredSetter
{
    protected ISvgStyle? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToISvgStyle();
    }
}