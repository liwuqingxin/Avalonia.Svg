using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.radialGradient))]
public class SvgRadialGradient : SvgTagBase, ISvgPaintServer, ISvgBrushProvider,
    ICXSetter,
    ICYSetter,
    IRSetter,
    IFXSetter,
    IFYSetter,
    IFRSetter,
    IGradientSpreadMethodSetter,
    IGradientUnitsSetter,
    IGradientTransformSetter
{
    private LightBrush? _brush;

    public double? CX
    {
        get;
        set;
    }
    public double? CY
    {
        get;
        set;
    }
    public double? R
    {
        get;
        set;
    }
    public double? FX
    {
        get;
        set;
    }
    public double? FY
    {
        get;
        set;
    }
    public double? FR
    {
        get;
        set;
    }
    public GradientSpreadMethod? GradientSpreadMethod
    {
        get;
        set;
    }
    public SvgUnit? GradientUnits
    {
        get;
        set;
    }
    public Transform? GradientTransform
    {
        get;
        set;
    }

    string? ISvgBrushProvider.Id
    {
        get => ((IIdSetter)this).Id;
        set => ((IIdSetter)this).Id = value;
    }

    LightBrush ISvgBrushProvider.GetBrush()
    {
        if (_brush != null)
        {
            return _brush;
        }

        //
        // ref https://www.w3.org/TR/SVG2/pservers.html#LinearGradientAttributes
        // If gradientUnits="userSpaceOnUse", ‘x1’, ‘y1’, ‘x2’, and ‘y2’ represent values
        // in the coordinate system that results from taking the current user coordinate
        // system in place at the time when the gradient element is referenced (i.e., the
        // user coordinate system for the element referencing the gradient element via a
        // fill or stroke property) and then applying the transform specified by attribute
        // ‘gradientTransform’. Percentages represent values relative to the current SVG
        // viewport.
        //
        // If gradientUnits="objectBoundingBox", the user coordinate system for attributes
        // ‘x1’, ‘y1’, ‘x2’ and ‘y2’ is established using the bounding box of the element
        // to which the gradient is applied (see Object bounding box units) and then applying
        // the transform specified by attribute ‘gradientTransform’. Percentages represent
        // values relative to the bounding box for the object.When gradientUnits =
        // "objectBoundingBox" and ‘gradientTransform’ is the identity matrix, the normal of
        // the linear gradient is perpendicular to the gradient vector in object bounding
        // box space(i.e., the abstract coordinate system where(0,0) is at the top/left of
        // the object bounding box and(1,1) is at the bottom/right of the object bounding
        // box). When the object's bounding box is not square, the gradient normal which is
        // initially perpendicular to the gradient vector within object bounding box space
        // may render non-perpendicular relative to the gradient vector in user space. If
        // the gradient vector is parallel to one of the axes of the bounding box, the
        // gradient normal will remain perpendicular. This transformation is due to
        // application of the non-uniform scaling transformation from bounding box space to
        // user coordinate system.
        //
        // TODO 在userSpaceOnUse下，暂时没有处理%值，应该从ViewBox中取Bounds，按照这个计算百分比
        var relativeUnit = GradientUnits is SvgUnit.objectBoundingBox or null
            ? RelativeUnit.Relative
            : RelativeUnit.Absolute;

        var spreadMethod = GradientSpreadMethod ?? global::Avalonia.Media.GradientSpreadMethod.Pad;
        var gradientStops = Children?.OfType<SvgStop>().Select(s => s.GradientStop).ToList() ?? new List<ImmutableGradientStop>();

        var cx = CX ?? 0.5;
        var cy = CY ?? 0.5;
        var rx = FX ?? cx;
        var ry = FY ?? cy;

        // Avalonia does not support different value for vertical and horizontal direction.
        // BUG radius of radial gradient in avalonia is determined by width of the element to be rendered.
        var r  = R  ?? 0.5;
        var fr = FR ?? 0d;

        if (FR is > 0 && FR < r)
        {
            gradientStops = gradientStops.Select(s =>
            {
                var offsetLength = ((r - fr) * s.Offset + fr) / r;
                return new ImmutableGradientStop(offsetLength, s.Color);
            }).ToList();
        }

        // ref https://www.w3.org/TR/SVG2/pservers.html#LinearGradientElementX1Attribute
        var gradientBrush = new LightRadialGradientBrush(
            gradientStops: gradientStops,
            opacity: 1,
            transform: GradientTransform,
            transformOrigin: null,
            spreadMethod: spreadMethod,
            center: new RelativePoint(cx, cy, relativeUnit),
            gradientOrigin: new RelativePoint(rx, ry, relativeUnit),
            radius:r)
        {
            GradientUnit = GradientUnits ?? SvgUnit.objectBoundingBox,
        };

        return _brush = gradientBrush;
    }
}