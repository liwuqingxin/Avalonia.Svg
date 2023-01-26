using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg
{
    [TagFactoryGenerator(nameof(SvgTags.a))]
    public class SvgA : SvgRenderable, ISvgContainer, ISvgRenderable
    {

    }
}
