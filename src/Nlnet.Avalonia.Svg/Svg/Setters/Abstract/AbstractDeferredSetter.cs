using System;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter with ability to set deferred value string.
/// </summary>
public abstract class AbstractDeferredSetter : ISvgSetter
{
    private string? _deferredValueString;

    public abstract void Set(ISvgTag tag);

    public abstract void InitializeValue(string setterValue);

    public virtual void InitializeDeferredValue(ISvgContext context, string deferredSetterValue)
    {
        throw new NotImplementedException($"Initializing deferred value for {this.GetType()} is not implemented.");
    }

    public void AddDeferredValueString(string valueString)
    {
        _deferredValueString = valueString;
    }

    public void ApplyDeferredValueString(ISvgContext context)
    {
        if (_deferredValueString == null)
        {
            return;
        }

        InitializeDeferredValue(context, _deferredValueString);
    }
}
