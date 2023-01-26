using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Any element type that can have a direct representation in the rendering tree, as a graphic,
    /// container, text, audio, or animation.
    /// It includes the following elements: ‘a’, ‘audio’, ‘canvas’, ‘circle’, ‘ellipse’, ‘foreignObject’,
    /// ‘g’, ‘iframe’, ‘image’, ‘line’, ‘path’, ‘polygon’, ‘polyline’, ‘rect’, ‘svg’, ‘switch’, ‘text’,
    /// ‘textPath’, ‘tspan’, ‘unknown’, ‘use’ and ‘video’; it also includes a ‘symbol’ element that is
    /// the instance root of a use-element shadow tree.
    /// </summary>
    /// https://www.w3.org/TR/SVG2/render.html#TermRenderableElement
    public interface IRenderable
    {

    }

    /// <summary>
    /// A graphics element that is defined by some combination of straight lines and curves.
    /// Specifically: ‘circle’, ‘ellipse’, ‘line’, ‘path’, ‘polygon’, ‘polyline’ and ‘rect’.
    /// </summary>
    /// https://www.w3.org/TR/SVG2/shapes.html#TermShapeElement
    public interface IShape
    {

    }

    /// <summary>
    /// One of the element types that can cause graphics to be drawn onto the target canvas.
    /// Specifically: ‘audio’, ‘canvas’, ‘circle’, ‘ellipse’, ‘foreignObject’, ‘iframe’, ‘image’,
    /// ‘line’, ‘path’, ‘polygon’, ‘polyline’, ‘rect’, ‘text’, ‘textPath’, ‘tspan’ and ‘video’.
    /// </summary>
    /// https://www.w3.org/TR/SVG2/struct.html#TermGraphicsElement
    public interface IGraphic
    {

    }
}
