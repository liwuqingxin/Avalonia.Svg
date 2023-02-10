using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// A light brush that draws with a radial gradient. It is almost like to <see cref="ImmutableRadialGradientBrush"/>
    /// </summary>
    public class LightRadialGradientBrush : 
        LightGradientBrush, 
        IRadialGradientBrush
    {
        public RelativePoint Center { get; set; }

        public RelativePoint GradientOrigin { get; set; }

        public double Radius { get; set; }

        /// <summary>
        /// Initializes a new instance of the Avalonia.Media.Immutable.LightRadialGradientBrush class.
        /// </summary>
        /// <param name="gradientStops">The gradient stops.</param>
        /// <param name="opacity">The opacity of the brush.</param>
        /// <param name="transform">The transform of the brush.</param>
        /// <param name="transformOrigin">The transform origin of the brush</param>
        /// <param name="spreadMethod">The spread method.</param>
        /// <param name="center">The start point for the gradient.</param>
        /// <param name="gradientOrigin">The location of the two-dimensional focal point that defines the beginning of the gradient.</param>
        /// <param name="radius">The horizontal and vertical radius of the outermost circle of the radial gradient.</param>
        public LightRadialGradientBrush(
            IReadOnlyList<ImmutableGradientStop> gradientStops,
            double                               opacity         = 1.0,
            ITransform?                          transform       = null,
            RelativePoint?                       transformOrigin = null,
            GradientSpreadMethod                 spreadMethod    = GradientSpreadMethod.Pad,
            RelativePoint?                       center          = null,
            RelativePoint?                       gradientOrigin  = null,
            double                               radius          = 0.5)
            : base(gradientStops, opacity, transform, transformOrigin, spreadMethod)
        {
            Center         = center         ?? RelativePoint.Center;
            GradientOrigin = gradientOrigin ?? RelativePoint.Center;
            Radius         = radius;
        }

        /// <summary>
        /// Initializes a new instance of the Avalonia.Media.Immutable.LightRadialGradientBrush class.
        /// </summary>
        /// <param name="source">The brush from which this brush's properties should be copied.</param>
        public LightRadialGradientBrush(IRadialGradientBrush source)
            : base(source)
        {
            Center = source.Center;
            GradientOrigin = source.GradientOrigin;
            Radius = source.Radius;
        }

        public override LightBrush Clone()
        {
            return new LightRadialGradientBrush(this);
        }
    }
}