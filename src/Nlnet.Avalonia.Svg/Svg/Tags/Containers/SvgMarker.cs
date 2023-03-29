using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.marker))]
public class SvgMarker : SvgContainer, ISvgContainer, IDef,
    IMarkerWidthSetter,
    IMarkerHeightSetter,
    IGradientUnitsSetter,
    IMarkerOrientSetter,
    IViewBoxSetter,
    IPreserveAspectRatioSetter
{
    public double? MarkerWidth
    {
        get;
        set;
    }
    public double? MarkerHeight
    {
        get;
        set;
    }
    public SvgUnit? GradientUnits
    {
        get;
        set;
    }
    public SvgMarkerOrient? MarkerOrient
    {
        get;
        set;
    }
    public ViewBox? ViewBox
    {
        get;
        set;
    }
    public PreserveAspectRatio? PreserveAspectRatio
    {
        get;
        set;
    }
}