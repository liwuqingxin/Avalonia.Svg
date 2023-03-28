using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Svg document interface. It can be rendered to ui with a <see cref="DrawingContext"/>.
    /// </summary>
    public interface ISvg
    {
        /// <summary>
        /// Render the svg document to ui.
        /// </summary>
        /// <param name="dc">A drawing context for rendering.</param>
        /// <param name="availableSize">The size that can be used to render. The origin is (0,0).</param>
        /// <param name="stretch">The stretch mode this svg will be rendered with.</param>
        /// <param name="showDiagnosis">Indicates if show some diagnosis information.</param>
        public void Render(DrawingContext dc, Size availableSize, Stretch stretch, bool showDiagnosis);

        /// <summary>
        /// Get the svg render size.
        /// </summary>
        /// <returns></returns>
        Size GetDesiredSize(Size availableSize);
    }
}
