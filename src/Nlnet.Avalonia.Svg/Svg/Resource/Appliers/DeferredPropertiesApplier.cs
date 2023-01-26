namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Deferred properties applier for <see cref="ISvgResourceApplier"/>.
/// </summary>
public class DeferredPropertiesApplier : ISvgResourceApplier
{
    public void Apply(ISvgTag tag, ISvgResourceCollector collector)
    {
        if (tag.DeferredProperties == null)
        {
            return;
        }

        foreach (var pair in tag.DeferredProperties)
        {
            var setter = SvgStyleSetterFactory.GetSetterFactory(pair.Key)?.CreateSetter();
            if (setter == null)
            {
                continue;
            }
            setter.InitializeDeferredValue(collector, pair.Value);
            setter.Set(tag);
        }
    }
}