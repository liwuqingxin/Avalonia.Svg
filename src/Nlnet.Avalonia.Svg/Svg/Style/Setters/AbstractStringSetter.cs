using System;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="string"/>.
/// </summary>
public abstract class AbstractStringSetter : ISvgStyleSetter
{
    protected string? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = setterValue;
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        throw new NotImplementedException($"Deferred string value is not implemented");
    }
}
