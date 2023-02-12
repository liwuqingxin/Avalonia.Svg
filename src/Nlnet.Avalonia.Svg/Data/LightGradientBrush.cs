using System.Collections.Generic;
using System.Collections.Immutable;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// A brush that draws with a gradient.
    /// </summary>
    public abstract class LightGradientBrush : LightBrush, IGradientBrush, IGradientInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Nlnet.Avalonia.Svg.LightGradientBrush" /> class.
        /// </summary>
        /// <param name="gradientStops">The gradient stops.</param>
        /// <param name="opacity">The opacity of the brush.</param>
        /// <param name="transform">The transform of the brush.</param>
        /// <param name="transformOrigin">The transform origin of the brush</param>
        /// <param name="spreadMethod">The spread method.</param>
        protected LightGradientBrush(
            IReadOnlyList<ImmutableGradientStop> gradientStops,
            double                               opacity,
            ITransform?                          transform,
            RelativePoint?                       transformOrigin,
            GradientSpreadMethod                 spreadMethod)
        {
            this.GradientStops   = gradientStops;
            this.Opacity         = opacity;
            this.Transform       = transform;
            this.TransformOrigin = transformOrigin ?? RelativePoint.TopLeft;
            this.SpreadMethod    = spreadMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Nlnet.Avalonia.Svg.LightGradientBrush" /> class.
        /// </summary>
        /// <param name="source">The brush from which this brush's properties should be copied.</param>
        protected LightGradientBrush(IGradientBrush source)
        {
            var gradientStops   = source.GradientStops.ToImmutableArray();
            var opacity         = source.Opacity;
            var transform1      = source.Transform;
            var transform2      = transform1?.ToImmutable();
            var transformOrigin = new RelativePoint?(source.TransformOrigin);
            var spreadMethod    = source.SpreadMethod;

            this.GradientStops   = gradientStops;
            this.Opacity         = opacity;
            this.Transform       = transform2;
            this.TransformOrigin = transformOrigin ?? RelativePoint.TopLeft;
            this.SpreadMethod    = spreadMethod;
        }

        public IReadOnlyList<IGradientStop> GradientStops { get; }

        public GradientSpreadMethod SpreadMethod { get; set; }



        #region IGradientInfo

        public GradientUnit Unit
        {
            get;
            set;
        }

        #endregion
    }
}
