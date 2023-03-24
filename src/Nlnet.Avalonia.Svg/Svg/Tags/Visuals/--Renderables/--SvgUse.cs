using System.Xml;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.use))]
public class SvgUse : SvgRenderable, ISvgRenderable, 
    IXSetter, 
    IYSetter, 
    IWidthSetter, 
    IHeightSetter
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
}
