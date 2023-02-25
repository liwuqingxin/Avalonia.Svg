using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.g))]
public class SvgGroup : SvgContainer, ISvgRenderable
{
    public bool RenderBySelf => true;

    public override void Render(DrawingContext dc)
    {
        if (Transform == null)
        {
            this.Children?.RenderRecursively(dc);
        }
        else
        {
            using (dc.PushTransformContainer())
            {
                using (dc.PushSetTransform(Transform.Value))
                {
                    this.Children?.RenderRecursively(dc);
                }
            }
        }
    }
}