using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg
{
    public class SvgContainer : SvgRenderable, ISvgContainer
    {
        private Rect? _rect;

        public LightBrush? Fill
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
        public LightBrush? Stroke
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

        public override Rect RenderBounds => _rect ??= GetContainerRenderBounds();

        private Rect GetContainerRenderBounds()
        {
            if (this.Children == null)
            {
                return Rect.Empty;
            }

            var l = double.MaxValue;
            var r = double.MinValue;
            var t = double.MaxValue;
            var b = double.MinValue;
            var boundsArray = this.Children.OfType<SvgRenderable>().Select(renderable => renderable.RenderBounds);
            foreach (var rect in boundsArray)
            {
                l = Math.Min(l, rect.Left);
                r = Math.Max(r, rect.Right);
                t = Math.Min(t, rect.Top);
                b = Math.Max(b, rect.Bottom);
            }

            return new Rect(l, t, r - l, b - t);
        }

        public override void Render(DrawingContext dc, ISvgContext ctx)
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

            using (dc.PushPostTransform(matrix))
            {
                using (dc.PushTransformContainer())
                {
                    foreach (var child in Children.OfType<ISvgRenderable>())
                    {
                        child.Render(dc, ctx);
                    }
                }
            }
        }
    }
}
