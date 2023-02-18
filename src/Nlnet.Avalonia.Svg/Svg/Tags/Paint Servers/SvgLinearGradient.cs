using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.linearGradient))]
public class SvgLinearGradient : SvgPaintServer, ISvgPaintServer, ISvgBrushProvider,
    IX1Setter,
    IX2Setter,
    IY1Setter,
    IY2Setter,
    IGradientSpreadMethodSetter,
    IGradientUnitsSetter,
    IGradientTransformSetter,
    IHrefSetter,
    IXHrefSetter
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
    string? IHrefSetter.Href
    {
        get;
        set;
    }
    public string? XHref
    {
        get;
        set;
    }



    protected override string? Href => ((IHrefSetter)this).Href ?? XHref;

    protected override void EnsureTemplateCore(ISvgContext context, SvgPaintServer paintServer)
    {
        var template = (SvgLinearGradient)paintServer;

        if (X1 == null && template.X1 != null) X1 = template.X1;
        if (X2 == null && template.X2 != null) X2 = template.X2;
        if (Y1 == null && template.Y1 != null) Y1 = template.Y1;
        if (Y2 == null && template.Y2 != null) Y2 = template.Y2;

        if (GradientUnits == null && template.GradientUnits != null)
        {
            GradientUnits = template.GradientUnits;
        }
        if (GradientTransform == null && template.GradientTransform != null)
        {
            GradientTransform = template.GradientTransform;
        }
        if (GradientSpreadMethod == null && template.GradientSpreadMethod != null)
        {
            GradientSpreadMethod = template.GradientSpreadMethod;
        }

        var stops = Children?.OfType<SvgStop>().ToList();
        var templateStops = template.Children?.OfType<SvgStop>().ToList();
        if ((stops == null || stops.Count == 0) && templateStops is { Count: >= 0 })
        {
            Children = template.Children;
        }
    }



    #region ISvgBrushProvider

    string? ISvgBrushProvider.Id
    {
        get => ((IIdSetter) this).Id;
        set => ((IIdSetter) this).Id = value;
    }

    LightBrush ISvgBrushProvider.GetBrush(ISvgContext context)
    {
        if (_brush != null)
        {
            return _brush;
        }

        EnsureTemplate(context);

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

    #endregion
}