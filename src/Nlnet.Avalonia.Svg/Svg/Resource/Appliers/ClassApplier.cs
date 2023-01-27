namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Class applier for <see cref="ISvgContextApplier"/>.
/// </summary>
public class ClassApplier : ISvgContextApplier
{
    public void Apply(ISvgTag tag, ISvgContext context)
    {
        if (tag is not IClassSetter setter)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(setter.Class))
        {
            return;
        }

        if (context.Styles.TryGetValue(setter.Class, out var style))
        {
            style.ApplyTo(tag);
        }
    }
}