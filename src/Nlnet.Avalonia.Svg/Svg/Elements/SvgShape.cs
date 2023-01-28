using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg
{
    public class SvgShape : SvgRenderable, ISvgShape,
        IFillSetter,
        IStrokeSetter,
        IStrokeWidthSetter
    {
        /// <summary>
        /// The original geometry that the svg describes.
        /// </summary>
        protected Geometry? OriginalGeometry;

        /// <summary>
        /// The geometry to render, which applied some transforms.
        /// </summary>
        protected Geometry? RenderGeometry;

        IBrush? IFillSetter.Fill { get; set; }

        IBrush? IStrokeSetter.Stroke { get; set; }

        double? IStrokeWidthSetter.StrokeWidth { get; set; }

        public override Rect Bounds => OriginalGeometry?.Bounds ?? Rect.Empty;

        public override Rect RenderBounds => RenderGeometry?.GetRenderBounds(GetPen()) ?? Rect.Empty;


        public override void ApplyTransforms()
        {
            if (OriginalGeometry == null)
            {
                return;
            }

            RenderGeometry = OriginalGeometry.Clone();
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

            var fill = this.GetPropertyValue<IFillSetter, IBrush>();

            void DoRender() => dc.DrawGeometry(fill, GetPen(), RenderGeometry);

            if (Opacity != null)
            {
                using (dc.PushOpacity(Opacity.Value))
                {
                    DoRender();
                }
            }
            else
            {
                DoRender();
            }
        }

        private IPen GetPen()
        {
            var stroke      = this.GetPropertyValue<IStrokeSetter, IBrush>();
            var strokeWidth = this.GetPropertyStructValue<IStrokeWidthSetter, double>();
            return new Pen(stroke, strokeWidth ?? 0);
        }
    }
}
