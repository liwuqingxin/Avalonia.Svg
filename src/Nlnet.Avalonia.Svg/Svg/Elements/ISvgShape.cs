namespace Nlnet.Avalonia.Svg;

/// <summary>
/// A graphics element that is defined by some combination of straight lines and curves.
/// Specifically: ‘circle’, ‘ellipse’, ‘line’, ‘path’, ‘polygon’, ‘polyline’ and ‘rect’.
/// </summary>
/// https://www.w3.org/TR/SVG2/shapes.html#TermShapeElement
public interface ISvgShape :
    IFillSetter,
    IFillRuleSetter,
    IFillOpacitySetter,
    IStrokeSetter,
    IStrokeOpacitySetter,
    IStrokeWidthSetter
{

}
