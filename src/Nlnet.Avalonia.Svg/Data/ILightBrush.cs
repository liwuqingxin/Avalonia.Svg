using Avalonia;
using Avalonia.Media;
using Avalonia.Metadata;
using System.ComponentModel;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Describes how an area is painted.
    /// </summary>
    [TypeConverter(typeof(BrushConverter))]
    [NotClientImplementable]
    public interface ILightBrush : IBrush
    {
        /// <summary>
        /// Gets the opacity of the brush.
        /// </summary>
        double Opacity { get; set; }

        /// <summary>
        /// Gets the transform of the brush.
        /// </summary>
        ITransform? Transform { get; set; }

        /// <summary>
        /// Gets the origin of the brushes <see cref="P:Avalonia.Media.IBrush.Transform" />
        /// </summary>
        RelativePoint TransformOrigin { get; set; }
    }
}
