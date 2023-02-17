using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.pattern))]
public class SvgPattern : SvgContainer, ISvgPaintServer, ISvgContainer,
    IXSetter,
    IYSetter,
    IWidthSetter,
    IHeightSetter,
    IPatternUnitsSetter,
    IPatternContentUnitsSetter,
    IPatternTransformSetter
//,href,preserveAspectRatio
{
    public double? X
    {
        get;
        set;
    }
    public double? Y
    {
        get;
        set;
    }
    public double? Width
    {
        get;
        set;
    }
    public double? Height
    {
        get;
        set;
    }
    public SvgUnit? PatternUnits
    {
        get;
        set;
    }
    public SvgUnit? PatternContentUnits
    {
        get;
        set;
    }
    public Transform? PatternTransform
    {
        get;
        set;
    }
}