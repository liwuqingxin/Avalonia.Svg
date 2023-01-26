namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Deferred setter value applier for <see cref="ISvgResourceApplier"/>.
/// </summary>
public class DeferredSetterValueApplier : ISvgResourceApplier
{
    public void Apply(ISvgTag tag, ISvgResourceCollector collector)
    {
        if (tag is not ISvgStyleProvider provider)
        {
            return;
        }

        var styles = provider.GetStyles();
        foreach (var style in styles)
        {
            style.Setters.ForEach(s => s.ApplyDeferredValueString(collector));
        }
    }
}
