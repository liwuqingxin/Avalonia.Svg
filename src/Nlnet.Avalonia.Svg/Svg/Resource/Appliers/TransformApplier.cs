namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Transform applier for <see cref="ISvgResourceApplier"/>.
/// </summary>
public class TransformApplier : ISvgResourceApplier
{
    public void Apply(ISvgTag tag, ISvgResourceCollector collector)
    {
        if (tag is not ISvgRenderable renderable)
        {
            return;
        }

        renderable.BuildRenderGeometry();
    }
}
