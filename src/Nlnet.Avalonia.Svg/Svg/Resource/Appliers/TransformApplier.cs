namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Transform applier for <see cref="ISvgContextApplier"/>.
/// </summary>
public class TransformApplier : ISvgContextApplier
{
    public void Apply(ISvgTag tag, ISvgContext context)
    {
        if (tag is not ISvgRenderable renderable)
        {
            return;
        }

        renderable.BuildRenderGeometry();
    }
}
