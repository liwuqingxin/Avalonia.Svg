using System;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

public abstract class AbstractTransformSetter : ISvgStyleSetter
{
    protected Transform? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = setterValue.ToTransform();
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        throw new NotImplementedException($"Deferred {nameof(Transform)} value is not implemented");
    }
}