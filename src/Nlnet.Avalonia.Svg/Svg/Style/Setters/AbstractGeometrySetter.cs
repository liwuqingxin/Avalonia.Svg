using System;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="Geometry"/>.
/// </summary>
public abstract class AbstractGeometrySetter : ISvgStyleSetter
{
    protected Geometry? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = setterValue.ToGeometry();
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        throw new NotImplementedException($"Deferred {nameof(Geometry)} value is not implemented");
    }
}
