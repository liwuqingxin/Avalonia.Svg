namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Group heritable properties applier for <see cref="ISvgResourceApplier"/>.
/// </summary>
public class GroupHeritablePropertiesApplier : ISvgResourceApplier
{
    // TODO 1. 改为应用所有属性；
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
            if (t is not ISvgRenderable renderable)
            {
                return;
            }

            //renderable.ApplyAncestorTransform(group.Transform);
        });
    }
}
