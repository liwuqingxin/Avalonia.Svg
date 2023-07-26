using System.Linq;
using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg
{
    public class SvgContainer : SvgRenderable, ISvgContainer
    {
        public IBrush? Fill
        {
            get;
            set;
        }
        public FillRule? FillRule
        {
            get;
            set;
        }
        public double? FillOpacity
        {
            get;
            set;
        }
        public IBrush? Stroke
        {
            get;
            set;
        }
        public double? StrokeOpacity
        {
            get;
            set;
        }
        public double? StrokeWidth
        {
            get;
            set;
        }
        public PenLineCap? StrokeLineCap
        {
            get;
            set;
        }
        public PenLineJoin? StrokeLineJoin
        {
            get;
            set;
        }
        public double? StrokeMiterLimit
        {
            get;
            set;
        }
        public DoubleList? StrokeDashArray
        {
            get;
            set;
        }
        public double? StrokeDashOffset
        {
            get;
            set;
        }

        protected override void RenderCore(DrawingContext dc, ISvgContext ctx)
        {
            if (this.Children == null || this is IDef)
            {
                return;
            }

            var matrix = Matrix.Identity;

            // Transform
            if (Transform != null)
            {
                matrix *= Transform.Value;
            }

            // X and Y
            if (this is IXSetter xSetter && this is IYSetter ySetter)
            {
                matrix *= Matrix.CreateTranslation(xSetter.X ?? 0, ySetter.Y ?? 0);
            }

            using (dc.PushOpacity(this.Opacity ?? 1))
            using (dc.PushTransform(matrix))
            using (dc.PushTransform(Matrix.Identity)) // TODO 测试是否必要
            {
                foreach (var child in Children.OfType<ISvgRenderable>())
                {
                    child.Render(dc, ctx);
                }
            }
        }
    }
}
