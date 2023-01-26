using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.clipPath))]
public class SvgClipPath : SvgTagBase, ISvgContainer,
    IIdSetter
{
    public string? Id { get; set; }
}