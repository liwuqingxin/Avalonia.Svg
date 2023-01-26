using System;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="Geometry"/>.
/// </summary>
public abstract class AbstractGeometrySetter : AbstractDeferredSetter
{
    protected Geometry? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToGeometry();
    }
}
