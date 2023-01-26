using System;

namespace Nlnet.Avalonia.Svg;

public abstract class AbstractPointsSetter : ISvgStyleSetter
{
    protected PointList? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = setterValue.ToPointList();
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        throw new NotImplementedException($"Deferred {nameof(PointList)} value is not implemented");
    }
}
