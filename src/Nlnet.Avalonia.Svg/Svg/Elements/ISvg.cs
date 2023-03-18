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
        /// <param name="dc"></param>
        /// <param name="availableSize"></param>
        public void Render(DrawingContext dc, Size availableSize);

        /// <summary>
        /// Get the svg render size.
        /// </summary>
        /// <returns></returns>
        Size GetDesiredSize(Size availableSize);
    }
}
