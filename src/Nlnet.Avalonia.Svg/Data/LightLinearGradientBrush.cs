using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// A brush that draws with a linear gradient. It is almost like to <see cref="ImmutableLinearGradientBrush"/>.
    /// </summary>
    public class LightLinearGradientBrush :
        LightGradientBrush,
        ILinearGradientBrush,
        IGradientBrush,
        IBrush
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Nlnet.Avalonia.Svg.LightLineGradientBrush" /> class.
        /// </summary>
        /// <param name="gradientStops">The gradient stops.</param>
        /// <param name="opacity">The opacity of the brush.</param>
        /// <param name="transform">The transform of the brush.</param>
        /// <param name="transformOrigin">The transform origin of the brush</param>
        /// <param name="spreadMethod">The spread method.</param>
        /// <param name="startPoint">The start point for the gradient.</param>
        /// <param name="endPoint">The end point for the gradient.</param>
        public LightLinearGradientBrush(
            IReadOnlyList<ImmutableGradientStop> gradientStops,
            double                               opacity         = 1.0,
            ITransform?                          transform       = null,
            RelativePoint?                       transformOrigin = null,
            GradientSpreadMethod                 spreadMethod    = GradientSpreadMethod.Pad,
            RelativePoint?                       startPoint      = null,
            RelativePoint?                       endPoint        = null)
            : base(gradientStops, opacity, transform, transformOrigin, spreadMethod)
        {
            var nullable = startPoint;
            this.StartPoint = nullable ?? RelativePoint.TopLeft;
            nullable        = endPoint;
            this.EndPoint   = nullable ?? RelativePoint.BottomRight;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Avalonia.Media.Immutable.LightLineGradientBrush" /> class.
        /// </summary>
        /// <param name="source">The brush from which this brush's properties should be copied.</param>
        public LightLinearGradientBrush(LightLinearGradientBrush source)
            : base(source)
        {
            this.StartPoint = source.StartPoint;
            this.EndPoint   = source.EndPoint;
        }

        /// <inheritdoc />
        public RelativePoint StartPoint { get; set; }

        /// <inheritdoc />
        public RelativePoint EndPoint { get; set; }

        public override LightBrush Clone()
        {
            return new LightLinearGradientBrush(this);
        }
    }
}