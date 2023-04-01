using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.Utils;

namespace Nlnet.Avalonia.Svg
{
    public abstract class SvgShape : SvgRenderable, ISvgShape
    {
        #region ISvgShape

        public Geometry? OriginalGeometry { get; private set; }

        #endregion



        #region Svg Properties

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

        #endregion



        public override Rect Bounds => OriginalGeometry?.Bounds ?? Rect.Empty;

        public sealed override void OnPropertiesFetched()
        {
            OriginalGeometry = OnCreateOriginalGeometry();
        }

        /// <summary>
        /// This function should return a <see cref="Geometry"/>? as the origin geometry.
        /// </summary>
        /// <returns></returns>
        protected abstract Geometry? OnCreateOriginalGeometry();

        /// <summary>
        /// Render the <see cref="ISvgShape"/>. In <see cref="SvgShape"/>, it renders the <see cref="RenderGeometry"/>.
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="ctx"></param>
        protected override void RenderCore(DrawingContext dc, ISvgContext ctx)
        {
            if (OriginalGeometry == null)
            {
                return;
            }

            var fill = this.GetPropertyValue<IFillSetter, LightBrush>()?.Clone();
            var fillOpacity = this.GetPropertyStructValue<IFillOpacitySetter, double>();

            ApplyBrushOpacity(fill, fillOpacity);
            ApplyBrushTransform(fill);

            using var stack = new StateStack();
            if (Opacity != null)
            {
                stack.Push(dc.PushOpacity(Opacity.Value));
            }
            if (Transform != null)
            {
                stack.Push(dc.PushPostTransform(Transform.Value));
                stack.Push(dc.PushTransformContainer());
            }

            dc.DrawGeometry(fill, GetPen(), OriginalGeometry);

            // Render Markers
            if (this is ISvgMarkerable markerable)
            {
                var marker = this.GetPropertyValue<IMarkerSetter, string>();

                var markerStart = this.GetPropertyValue<IMarkerStartSetter, string>() ?? marker;
                if (TryGetMarker(markerStart, ctx, out var marker1))
                {
                    markerable.RenderMarkerStart(dc, ctx, marker1!);
                }

                var markerMid = this.GetPropertyValue<IMarkerMidSetter, string>() ?? marker;
                if (TryGetMarker(markerMid, ctx, out var marker2))
                {
                    markerable.RenderMarkerMid(dc, ctx, marker2!);
                }

                var markerEnd = this.GetPropertyValue<IMarkerEndSetter, string>() ?? marker;
                if (TryGetMarker(markerEnd, ctx, out var marker3))
                {
                    markerable.RenderMarkerEnd(dc, ctx, marker3!);
                }
            }
        }

        private bool TryGetMarker(string? href, ISvgContext ctx, out SvgMarker? marker)
        {
            marker = null;
            return href != null && href.TryParseUrl(out var id, out _) && ctx.Markers.TryGetValue(id, out marker);
        }

        public override object Clone()
        {
            var clone = base.Clone();

            if (clone is SvgShape shape)
            {
                shape.OriginalGeometry = this.OriginalGeometry;
                shape._pen = this._pen;
            }

            return clone;
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
            dashOffset /= strokeWidth;

            var dashStyle = new ImmutableDashStyle(dashArray ?? new DoubleList(), dashOffset);

            ApplyBrushOpacity(stroke, strokeOpacity);
            ApplyBrushTransform(stroke);

            return _pen = new ImmutablePen(stroke, strokeWidth, dashStyle, lineCap, lineJoin, miterLimit);
        }

        private void ApplyBrushOpacity(LightBrush? lightBrush, double opacity)
        {
            if (lightBrush == null)
            {
                return;
            }

            lightBrush.Opacity = opacity;
        }

        private void ApplyBrushTransform(LightBrush? lightBrush)
        {
            if (lightBrush is not LightGradientBrush gradientBrush)
            {
                return;
            }

            var immutableTransform = GetBrushTransform(gradientBrush.GradientUnit);
            if (immutableTransform == null)
            {
                return;
            }

            if (lightBrush.Transform != null)
            {
                // Transform must be immutable, or the element rendered will be flickering.
                immutableTransform = new ImmutableTransform(lightBrush.Transform.Value * immutableTransform.Value);
            }

            lightBrush.Transform = immutableTransform;
        }

        /// <summary>
        /// Get the immutable transform for brush. Note that the brush coordinate system is
        /// relative to the origin of the <see cref="DrawingContext"/> canvas in avalonia.
        /// </summary>
        /// <param name="gradientUnit"></param>
        /// <returns></returns>
        protected virtual ImmutableTransform? GetBrushTransform(SvgUnit gradientUnit)
        {
            switch (gradientUnit)
            {
                // The userSpaceOnUse uses container's bounds as coordinate system.
                // The objectBoundingBox uses the element that is to be rendered with gradient
                // brush as coordinate system.
                case SvgUnit.objectBoundingBox:
                {
                    var t1 = Transform?.Value ?? Matrix.Identity;
                    var t2 = new Matrix(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, this.Bounds.Left, this.Bounds.Top, 1.0);
                    return new ImmutableTransform(t1 * t2);
                }
                case SvgUnit.userSpaceOnUse:
                {
                    var t1 = Transform?.Value ?? Matrix.Identity;
                    return new ImmutableTransform(t1);
                }
            }

            return null;
        }
    }
}
