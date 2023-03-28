using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.defs))]
public class SvgDefs : SvgContainer, ISvgContainer, IDef
{
    
}