using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

// TODO As Mask resource.

[TagFactoryGenerator(nameof(SvgTags.mask))]
public class SvgMask : SvgContainer, ISvgContainer, 
    IIdSetter
{
    public string? Id { get; set; }
}