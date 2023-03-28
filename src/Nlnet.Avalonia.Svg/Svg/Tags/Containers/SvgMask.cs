using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.mask))]
public class SvgMask : SvgContainer, ISvgContainer, IDef,
    IIdSetter
{
    public string? Id { get; set; }
}