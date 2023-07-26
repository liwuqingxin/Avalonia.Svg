namespace Nlnet.Avalonia.Svg;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

/// <summary>
/// Svg property default value strings.
/// </summary>
public static class SvgDefaultValues
{
    public const string Null                      = "null";
    public const string Zero                      = "0d";
    public const string Half                      = "0.5d";
    public const string One                       = "1d";
    public const string Three                     = "3d";
    public const string Four                      = "4d";
    public const string BrushBlack                = "Brushes.Black";
    public const string BrushTransparent          = "Brushes.Transparent";
    public const string FillRuleNonZero           = "global::Avalonia.Media.FillRule.NonZero";
    public const string PenLineCapNonZero         = "global::Avalonia.Media.PenLineCap.Flat";
    public const string PenLineJoinNonZero        = "global::Avalonia.Media.PenLineJoin.Miter";
    public const string GradientSpreadMethodPad   = "global::Avalonia.Media.GradientSpreadMethod.Pad";
    public const string SvgUnitObjectBoundingBox  = "global::Nlnet.Avalonia.Svg.SvgUnit.objectBoundingBox";
    public const string SvgMarkerUnitsStrokeWidth = "global::Nlnet.Avalonia.Svg.SvgMarkerUnits.strokeWidth";
    public const string SvgMarkerOrientAuto       = "global::Nlnet.Avalonia.Svg.SvgMarkerOrient.Default";
    public const string RefXDefault               = "global::Nlnet.Avalonia.Svg.RefX.Default";
    public const string RefYDefault               = "global::Nlnet.Avalonia.Svg.RefY.Default";
}

/// <summary>
/// All svg property definitions.
/// </summary>
public static class SvgProperties
{
    public const string Href                 = "href";
    public const string XHref                = "xlink:href";
    public const string Class                = "class";
    public const string Id                   = "id";
    public const string Version              = "version";
    public const string ViewBox              = "viewBox";
    public const string Style                = "style";
    public const string Type                 = "type";
    public const string Data                 = "d";
    public const string Offset               = "offset";
    public const string Opacity              = "opacity";
    public const string Points               = "points";
    public const string Fill                 = "fill";
    public const string FillRule             = "fill-rule";
    public const string FillOpacity          = "fill-opacity";
    public const string Stroke               = "stroke";
    public const string StrokeOpacity        = "stroke-opacity";
    public const string StrokeWidth          = "stroke-width";
    public const string StrokeLineCap        = "stroke-linecap";
    public const string StrokeLineJoin       = "stroke-linejoin";
    public const string StrokeMiterLimit     = "stroke-miterlimit";
    public const string StrokeDashArray      = "stroke-dasharray";
    public const string StrokeDashOffset     = "stroke-dashoffset";
    public const string StopOpacity          = "stop-opacity";
    public const string StopColor            = "stop-color";
    public const string CX                   = "cx";
    public const string CY                   = "cy";
    public const string RX                   = "rx";
    public const string RY                   = "ry";
    public const string FX                   = "fx";
    public const string FY                   = "fy";
    public const string X                    = "x";
    public const string Y                    = "y";
    public const string X1                   = "x1";
    public const string Y1                   = "y1";
    public const string X2                   = "x2";
    public const string Y2                   = "y2";
    public const string R                    = "r";
    public const string FR                   = "fr";
    public const string Width                = "width";
    public const string Height               = "height";
    public const string Transform            = "transform";
    public const string GradientSpreadMethod = "spreadMethod";
    public const string GradientUnits        = "gradientUnits";
    public const string GradientTransform    = "gradientTransform";
    public const string PatternUnits         = "patternUnits";
    public const string PatternContentUnits  = "patternContentUnits";
    public const string PatternTransform     = "patternTransform";
    public const string PreserveAspectRatio  = "preserveAspectRatio";
    public const string ClipPath             = "clip-path";
    public const string Mask                 = "mask";
    public const string MarkerStart          = "marker-start";
    public const string MarkerEnd            = "marker-end";
    public const string MarkerMid            = "marker-mid";
    public const string Marker               = "marker";
    public const string MarkerHeight         = "markerHeight";
    public const string MarkerWidth          = "markerWidth";
    public const string MarkerUnits          = "markerUnits";
    public const string MarkerOrient         = "orient";
    public const string RefX                 = "refX";
    public const string RefY                 = "refY";
}

/// <summary>
/// All svg tag definitions.
/// </summary>
public enum SvgTags
{
    /// <summary>
    /// Represent a tag that is invalid or ineffective.
    /// </summary>
    ignore,

    a,
    altGlyph,
    altGlyphDef,
    altGlyphItem,
    animate,
    animateColor,
    animateMotion,
    animateTransform,
    circle,
    clipPath,
    color_profile,
    cursor,
    definition_src,
    defs,
    desc,
    ellipse,
    feBlend,
    feColorMatrix,
    feComponentTransfer,
    feComposite,
    feConvolveMatrix,
    feDiffuseLighting,
    feDisplacementMap,
    feDistantLight,
    feFlood,
    feFuncA,
    feFuncB,
    feFuncG,
    feFuncR,
    feGaussianBlur,
    feImage,
    feMerge,
    feMergeNode,
    feMorphology,
    feOffset,
    fePointLight,
    feSpecularLighting,
    feSpotLight,
    feTile,
    feTurbulence,
    filter,
    font,
    [Name("font-face")]
    font_face,
    [Name("font-face-format")]
    font_face_format,
    [Name("font-face-name")]
    font_face_name,
    [Name("font-face-src")]
    font_face_src,
    [Name("font-face-uri")]
    font_face_uri,
    foreignObject,
    g,
    glyph,
    glyphRef,
    hkern,
    image,
    line,
    linearGradient,
    marker,
    mask,
    metadata,
    [Name("missing-glyph")]
    missing_glyph,
    mpath,
    path,
    pattern,
    polygon,
    polyline,
    radialGradient,
    rect,
    script,
    set,
    stop,
    style,
    svg,
    [Name("switch")]
    _switch,
    symbol,
    text,
    textPath,
    title,
    tref,
    tspan,
    use,
    view,
    vkern,
    unknown,
}
