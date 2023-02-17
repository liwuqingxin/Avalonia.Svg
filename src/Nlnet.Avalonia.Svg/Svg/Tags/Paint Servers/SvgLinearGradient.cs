using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.linearGradient))]
public class SvgLinearGradient : SvgTagBase, ISvgPaintServer, ISvgBrushProvider,
    IX1Setter,
    IX2Setter,
    IY1Setter,
    IY2Setter,
    IGradientSpreadMethodSetter,
    IGradientUnitsSetter,
    IGradientTransformSetter
{
    private LightBrush? _brush;

    public double? X1
    {
        get;
        set;
    }
    public double? X2
    {
        get;
        set;
    }
    public double? Y1
    {
        get;
        set;
    }
    public double? Y2
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
        get => ((IIdSetter) this).Id;
        set => ((IIdSetter) this).Id = value;
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

        // ref https://www.w3.org/TR/SVG2/pservers.html#LinearGradientElementX1Attribute
        var gradientBrush = new LightLinearGradientBrush(
            gradientStops: Children?.OfType<SvgStop>().Select(s => s.GradientStop).ToList() ?? new List<ImmutableGradientStop>(),
            opacity: 1,
            transform: GradientTransform,
            transformOrigin: null,
            spreadMethod: GradientSpreadMethod ?? global::Avalonia.Media.GradientSpreadMethod.Pad,
            startPoint: new RelativePoint(X1 ?? 0, Y1 ?? 0, relativeUnit),
            endPoint: new RelativePoint(X2   ?? 1, Y2 ?? 0, relativeUnit))
        {
            GradientUnit = GradientUnits ?? SvgUnit.objectBoundingBox,
        };

        return _brush = gradientBrush;
    }
}