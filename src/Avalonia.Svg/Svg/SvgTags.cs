namespace Avalonia.Svg;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

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
    @switch,
    symbol,
    text,
    textPath,
    title,
    tref,
    tspan,
    use,
    view,
    vkern,
}
