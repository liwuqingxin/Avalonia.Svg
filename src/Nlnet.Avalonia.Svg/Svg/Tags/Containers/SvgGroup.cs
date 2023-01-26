using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.g))]
public class SvgGroup : SvgRenderable, ISvgContainer, ISvgRenderable
{
    public bool RenderBySelf => true;

    public SvgGroup()
    {
        //ResourceAppliers = new List<ISvgResourceApplier>()
        //{
        //    new GroupHeritablePropertiesApplier(),
        //};
    }

    public override void Render(DrawingContext dc)
    {
        using (dc.PushTransformContainer())
        {
            using (dc.PushSetTransform(Transform?.Value ?? Matrix.Identity))
            {
                this.Children?.Render(dc);
            }
        }
    }
}