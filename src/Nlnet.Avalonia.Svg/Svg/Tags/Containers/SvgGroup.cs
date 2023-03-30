using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.g))]
public class SvgGroup : SvgContainer, ISvgRenderable, 
    IMarkerStartSetter,
    IMarkerEndSetter,
    IMarkerMidSetter,
    IMarkerSetter
{
    public string? MarkerStart
    {
        get;
        set;
    }
    public string? MarkerEnd
    {
        get;
        set;
    }
    public string? MarkerMid
    {
        get;
        set;
    }
    public string? Marker
    {
        get;
        set;
    }
}