using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace Nlnet.Avalonia.Svg
{
    // TODO 属性值优先级问题：Presentation Property Setting vs CSS Style Setting；
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

        LightBrush? IFillSetter.Fill { get; set; }

        FillRule? IFillRuleSetter.FillRule { get; set; }

        double? IFillOpacitySetter.FillOpacity { get; set; }

        LightBrush? IStrokeSetter.Stroke { get; set; }

        double? IStrokeOpacitySetter.StrokeOpacity { get; set; }

        double? IStrokeWidthSetter.StrokeWidth { get; set; }

        PenLineCap? IStrokeLineCapSetter.StrokeLineCap { get; set; }

        PenLineJoin? IStrokeLineJoinSetter.StrokeLineJoin { get; set; }

        double? IStrokeMiterLimitSetter.StrokeMiterLimit { get; set; }

        DoubleList? IStrokeDashArraySetter.StrokeDashArray { get; set; }

        double? IStrokeDashOffsetSetter.StrokeDashOffset { get; set; }

        public override Rect Bounds => OriginalGeometry?.Bounds ?? Rect.Empty;

        public override Rect RenderBounds => RenderGeometry?.GetRenderBounds(GetPen()) ?? Rect.Empty;


        protected SvgShape()
        {
            this.TryAddApplier(new ClassApplier());
            this.TryAddApplier(new DeferredPropertiesApplier());
        }


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

            var fill        = this.GetPropertyValue<IFillSetter, LightBrush>()?.Clone();
            var fillRule    = this.GetPropertyStructValue<IFillRuleSetter, FillRule>();
            var fillOpacity = this.GetPropertyStructValue<IFillOpacitySetter, double>();
            
            // TODO 应当计算位置、偏移量来控制色彩的偏移；
            // TODO 相对坐标计算标准不一样
            if (fill != null)
            {
                fill.Opacity   = fillOpacity;
                // TODO 这里放开会闪烁；
                fill.Transform = Transform;
            }

            RenderGeometry.FillRule = fillRule;

            using (dc.PushOpacity(Opacity ?? 1d))
            {
                dc.DrawGeometry(fill, GetPen(), RenderGeometry);
            }
        }

        private IPen? _pen;

        private IPen GetPen()
        {
            if (_pen != null)
            {
                return _pen;
            }

            var stroke        = this.GetPropertyValue<IStrokeSetter, LightBrush>()?.Clone();
            var strokeOpacity = this.GetPropertyStructValue<IStrokeOpacitySetter, double>();
            var strokeWidth   = this.GetPropertyStructValue<IStrokeWidthSetter, double>();
            var lineCap       = this.GetPropertyStructValue<IStrokeLineCapSetter, PenLineCap>();
            var lineJoin      = this.GetPropertyStructValue<IStrokeLineJoinSetter, PenLineJoin>();
            var miterLimit    = this.GetPropertyStructValue<IStrokeMiterLimitSetter, double>();
            var dashArray     = this.GetPropertyValue<IStrokeDashArraySetter, DoubleList>();
            var dashOffset    = this.GetPropertyStructValue<IStrokeDashOffsetSetter, double>();

            // In avalonia, the stroke width will effect the dash array. We should eliminate that factor.
            if (dashArray != null)
            {
                for (var i = 0; i < dashArray.Count; i++)
                {
                    dashArray[i] /= strokeWidth;
                }
            }

            var dashStyle = new ImmutableDashStyle(dashArray ?? new DoubleList(), dashOffset);

            if (stroke != null)
            {
                stroke.Opacity = strokeOpacity;
            }

            return _pen = new ImmutablePen(stroke, strokeWidth, dashStyle, lineCap, lineJoin, miterLimit);
        }
    }
}
