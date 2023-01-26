using System;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="double"/>.
/// </summary>
public abstract class AbstractDoubleSetter : ISvgStyleSetter
{
    protected double? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = setterValue.ToDouble();
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        throw new NotImplementedException("Deferred double value is not implemented");
    }
}
