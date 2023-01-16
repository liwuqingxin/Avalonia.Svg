namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Class applier for <see cref="ISvgResourceApplier"/>.
/// </summary>
public class ClassApplier : ISvgResourceApplier
{
    public void Apply(ISvgTag tag, ISvgResourceCollector collector)
    {
        if (tag is not IClassSetter setter)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(setter.Class))
        {
            return;
        }

        if (collector.Styles.TryGetValue(setter.Class, out var style))
        {
            style.ApplyTo(tag);
        }
    }
}
