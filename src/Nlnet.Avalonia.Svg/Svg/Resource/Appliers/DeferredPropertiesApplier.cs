﻿namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Deferred properties applier for <see cref="ISvgContextApplier"/>.
/// </summary>
public class DeferredPropertiesApplier : ISvgContextApplier
{
    public static DeferredPropertiesApplier Instance { get; } = new();

    public void Apply(ISvgTag tag, ISvgContext context)
    {
        if (tag.DeferredProperties == null)
        {
            return;
        }

        foreach (var pair in tag.DeferredProperties)
        {
            var setter = SvgSetterFactory.GetSetterFactory(pair.Key)?.CreateSetter();
            if (setter == null)
            {
                continue;
            }
            setter.InitializeDeferredValue(context, pair.Value);
            setter.Set(tag);
        }
    }
}