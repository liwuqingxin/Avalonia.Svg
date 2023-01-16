using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;
using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.g))]
public class SvgGroup : SvgTagBase, 
    ITransformSetter
{
    public Transform? Transform { get; set; }

    public SvgGroup()
    {
        ResourceAppliers = new List<ISvgResourceApplier>()
        {
            new GroupTransformApplier(),
        };
    }
}