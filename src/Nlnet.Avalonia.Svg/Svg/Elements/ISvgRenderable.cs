using System.Collections;
using System.Collections.Generic;
using Avalonia.Media;
using Avalonia;

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
    public interface ISvgRenderable
    {
        /// <summary>
        /// Original bounds of the <see cref="ISvgRenderable"/>.
        /// </summary>
        public Rect Bounds { get; }

        /// <summary>
        /// Rendered bounds of the <see cref="ISvgRenderable"/>. It takes stroke into account.
        /// </summary>
        public Rect RenderBounds { get; }

        /// <summary>
        /// Apply transforms.
        /// </summary>
        public void ApplyTransforms(Stack<Matrix> transformsContext);

        /// <summary>
        /// Render the <see cref="ISvgRenderable"/>.
        /// </summary>
        /// <param name="dc"></param>
        public void Render(DrawingContext dc);
    }
}
