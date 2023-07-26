using System;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg.Sample
{
    public class Mesh : Decorator
    {
        public double BlockSize
        {
            get { return GetValue(BlockSizeProperty); }
            set { SetValue(BlockSizeProperty, value); }
        }
        public static readonly StyledProperty<double> BlockSizeProperty = AvaloniaProperty
            .Register<Mesh, double>(nameof(BlockSize), 10d);

        public IBrush? BlockBrush
        {
            get { return GetValue(BlockBrushProperty); }
            set { SetValue(BlockBrushProperty, value); }
        }
        public static readonly StyledProperty<IBrush?> BlockBrushProperty = AvaloniaProperty
            .Register<Mesh, IBrush?>(nameof(BlockBrush), Brushes.DarkGray);

        public double BlockOpacity
        {
            get { return GetValue(BlockOpacityProperty); }
            set { SetValue(BlockOpacityProperty, value); }
        }
        public static readonly StyledProperty<double> BlockOpacityProperty = AvaloniaProperty
            .Register<Mesh, double>(nameof(BlockOpacity), 0.2d, false, BindingMode.Default, null, CoerceBlockOpacity);
        private static double CoerceBlockOpacity(AvaloniaObject sender, double baseValue)
        {
            return baseValue switch
            {
                > 1 => 1,
                < 0 => 0,
                _ => baseValue
            };
        }

        public bool UseMesh
        {
            get { return GetValue(UseMeshProperty); }
            set { SetValue(UseMeshProperty, value); }
        }
        public static readonly StyledProperty<bool> UseMeshProperty = AvaloniaProperty
            .Register<Mesh, bool>(nameof(UseMesh), true);

        public IBrush? BorderBrush
        {
            get { return GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }
        public static readonly StyledProperty<IBrush?> BorderBrushProperty = AvaloniaProperty
            .Register<Mesh, IBrush?>(nameof(BorderBrush));

        public Thickness BorderThickness
        {
            get { return GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }
        public static readonly StyledProperty<Thickness> BorderThicknessProperty = AvaloniaProperty
            .Register<Mesh, Thickness>(nameof(BorderThickness));



        static Mesh()
        {
            AffectsRender<Mesh>(BlockSizeProperty);
            AffectsRender<Mesh>(BlockBrushProperty);
            AffectsRender<Mesh>(BlockOpacityProperty);
            AffectsRender<Mesh>(UseMeshProperty);
        }

        public override void Render(DrawingContext context)
        {
            if (UseMesh)
            {
                var data = GetPath(BlockSize, this.Bounds.Width, this.Bounds.Height);
                var brush = BlockBrush;
                using (context.PushOpacity(BlockOpacity))
                {
                    context.DrawGeometry(brush, null, data);
                }

                if (BorderBrush != null)
                {
                    context.DrawRectangle(null, new Pen(BorderBrush, BorderThickness.Top), this.Bounds);
                }
            }

            base.Render(context);
        }

        private static Geometry GetPath(double dpi, double width, double height)
        {
            var xCount = (int)Math.Ceiling(width / dpi);
            var yCount = (int)Math.Ceiling(height / dpi);

            var builder = new StringBuilder();

            for (var r = 0; r < yCount; r++)
            {
                for (var c = 0; c < xCount; c++)
                {
                    if (r % 2 != 0)
                    {
                        c++;
                    }
                    var x0 = c * dpi;
                    var y0 = r * dpi;
                    builder.Append($"M{x0},{y0} {x0 + dpi},{y0} {x0 + dpi},{y0 + dpi} {x0},{y0 + dpi} {x0},{y0} ");
                    if (r % 2 == 0)
                    {
                        c++;
                    }
                }
            }

            return Geometry.Parse(builder.ToString());
        }
    }
}
