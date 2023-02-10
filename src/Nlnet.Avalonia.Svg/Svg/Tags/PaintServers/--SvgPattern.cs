using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.pattern))]
public class SvgPattern : SvgContainer, ISvgPaintServer, ISvgContainer
{
    
}