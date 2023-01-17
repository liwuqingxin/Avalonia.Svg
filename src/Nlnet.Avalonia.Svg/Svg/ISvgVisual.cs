using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Represent a visual that can be rendered to ui. It should define a <see cref="Bounds"/> and a <see cref="RenderBounds"/>
    /// to be the render area. Also it should render itself.
    /// </summary>
    public interface ISvgVisual : IOpacitySetter, ITransformSetter, IFillSetter, IStrokeSetter, IStrokeWidthSetter
    {
        /// <summary>
        /// Original bounds of the <see cref="ISvgVisual"/>.
        /// </summary>
        public Rect Bounds { get; }

        /// <summary>
        /// Rendered bounds of the <see cref="ISvgVisual"/>. After the svg loaded, it will normalize it self and do some transform.
        /// So the <see cref="RenderBounds"/> is different to <see cref="Bounds"/> normally.
        /// </summary>
        public Rect RenderBounds { get; }

        /// <summary>
        /// Render the <see cref="ISvgVisual"/>.
        /// </summary>
        /// <param name="dc"></param>
        public void Render(DrawingContext dc);

        /// <summary>
        /// Apply a global transform that is from svg or alignment to this <see cref="ISvgVisual"/>.
        /// </summary>
        /// <param name="transform"></param>
        void ApplyGlobalTransform(Transform transform);

        /// <summary>
        /// Apply a transform from ancestor group to this <see cref="ISvgVisual"/>.
        /// </summary>
        /// <param name="transform"></param>
        void ApplyAncestorTransform(Transform transform);
    }
}
