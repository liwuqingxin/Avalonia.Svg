namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Style applier for <see cref="ISvgContextApplier"/>.
/// </summary>
public class StyleApplier : ISvgContextApplier
{
    public void Apply(ISvgTag tag, ISvgContext context)
    {
        foreach (var style in context.Styles)
        {
            // TODO Do not consider priority here. Should be repaired.
            if (style.Match(tag))
            {
                style.ApplyTo(tag);
            }
        }
    }
}