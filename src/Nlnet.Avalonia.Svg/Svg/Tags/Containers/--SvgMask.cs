using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

// TODO As Mask resource.

[TagFactoryGenerator(nameof(SvgTags.mask))]
public class SvgMask : SvgTagBase, ISvgContainer, 
    IIdSetter,
    IFillSetter
{
    public string? Id   { get; set; }
    public IBrush? Fill { get; set; }
}