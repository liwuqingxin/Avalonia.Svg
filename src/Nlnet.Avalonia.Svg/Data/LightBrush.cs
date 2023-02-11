using Avalonia;
using Avalonia.Media;
using System.ComponentModel;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Describes how an area is painted.
    /// </summary>
    [TypeConverter(typeof(BrushConverter))]
    public abstract class LightBrush : IBrush
    {
        /// <summary>
        /// Gets the opacity of the brush.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets the transform of the brush.
        /// </summary>
        public ITransform? Transform { get; set; }

        /// <summary>
        /// Gets the origin of the brushes <see cref="P:Avalonia.Media.IBrush.Transform" />.
        /// </summary>
        public RelativePoint TransformOrigin { get; set; }

        /// <summary>
        /// Clone a <see cref="LightBrush"/>.
        /// </summary>
        /// <returns></returns>
        public abstract LightBrush Clone();
    }
}
