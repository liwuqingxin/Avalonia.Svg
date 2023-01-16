namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Group transform applier for <see cref="ISvgResourceApplier"/>.
/// </summary>
public class GroupTransformApplier : ISvgResourceApplier
{
    public void Apply(ISvgTag tag, ISvgResourceCollector collector)
    {
        if (tag is not SvgGroup group)
        {
            return;
        }
        if (group.Transform == null)
        {
            return;
        }

        group.VisitSvgTagTree(t =>
        {
            if (t is not ISvgVisual visual)
            {
                return;
            }

            visual.ApplyAncestorTransform(group.Transform);
        });
    }
}
