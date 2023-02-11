using System;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for all enum type.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AbstractEnumSetter<T> : AbstractDeferredSetter where T : Enum
{
    public T? Value { get; set; }
}
