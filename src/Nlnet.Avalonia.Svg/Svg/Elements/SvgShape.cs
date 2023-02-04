using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg
{
    public class SvgShape : SvgRenderable, ISvgShape
    {
        /// <summary>
        /// The original geometry that the svg describes.
        /// </summary>
        protected Geometry? OriginalGeometry;

        /// <summary>
        /// The geometry to render, which applied some transforms.
        /// </summary>
        protected GeometryGroup? RenderGeometry;

        IBrush? IFillSetter.Fill { get; set; }

        FillRule? IFillRuleSetter.FillRule { get; set; }

        double? IFillOpacitySetter.FillOpacity { get; set; }

        IBrush? IStrokeSetter.Stroke { get; set; }

        double? IStrokeOpacitySetter.StrokeOpacity { get; set; }

        double? IStrokeWidthSetter.StrokeWidth { get; set; }

        PenLineCap? IStrokeLineCapSetter.StrokeLineCap { get; set; }

        PenLineJoin? IStrokeLineJoinSetter.StrokeLineJoin { get; set; }

        double? IStrokeMiterLimitSetter.StrokeMiterLimit { get; set; }

        public override Rect Bounds => OriginalGeometry?.Bounds ?? Rect.Empty;

        public override Rect RenderBounds => RenderGeometry?.GetRenderBounds(GetPen()) ?? Rect.Empty;


        public override void ApplyTransforms()
        {
            if (OriginalGeometry == null)
            {
                return;
            }

            RenderGeometry = new GeometryGroup();
            RenderGeometry.Children.Add(OriginalGeometry);
            RenderGeometry.Transform = Transform;
        }

        /// <summary>
        /// Render the <see cref="ISvgShape"/>. In <see cref="SvgShape"/>, it renders the <see cref="RenderGeometry"/>.
        /// </summary>
        /// <param name="dc"></param>
        public override void Render(DrawingContext dc)
        {
            if (RenderGeometry == null)
            {
                return;
            }

            var fill        = this.GetPropertyValue<IFillSetter, IBrush>();
            var fillRule    = this.GetPropertyStructValue<IFillRuleSetter, FillRule>();
            var fillOpacity = this.GetPropertyStructValue<IFillOpacitySetter, double>();

            switch (fill)
            {
                case Brush brush:
                    brush.Opacity = fillOpacity;
                    break;
                case ImmutableColorSolidColorBrush solidColorBrush:
                    solidColorBrush.Opacity = fillOpacity;
                    break;
            }

            RenderGeometry.FillRule = fillRule;

            using (dc.PushOpacity(Opacity ?? 1d))
            {
                dc.DrawGeometry(fill, GetPen(), RenderGeometry);
            }
        }

        private IPen GetPen()
        {
            var stroke        = this.GetPropertyValue<IStrokeSetter, IBrush>();
            var strokeOpacity = this.GetPropertyStructValue<IStrokeOpacitySetter, double>();
            var strokeWidth   = this.GetPropertyStructValue<IStrokeWidthSetter, double>();
            var lineCap       = this.GetPropertyStructValue<IStrokeLineCapSetter, PenLineCap>();
            var lineJoin      = this.GetPropertyStructValue<IStrokeLineJoinSetter, PenLineJoin>();
            var miterLimit         = this.GetPropertyStructValue<IStrokeMiterLimitSetter, double>();

            switch (stroke)
            {
                case Brush brush:
                    brush.Opacity = strokeOpacity;
                    break;
                case ImmutableColorSolidColorBrush solidColorBrush:
                    solidColorBrush.Opacity = strokeOpacity;
                    break;
            }
            
            return new Pen(stroke, strokeWidth, null, lineCap, lineJoin, miterLimit);
        }
    }
}
