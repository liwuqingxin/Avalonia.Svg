using System;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg.Sample
{
    public class Mesh : Border
    {
        public double BlockSize
        {
            get { return GetValue(BlockSizeProperty); }
            set { SetValue(BlockSizeProperty, value); }
        }
        public static readonly StyledProperty<double> BlockSizeProperty = AvaloniaProperty
            .Register<Mesh, double>(nameof(BlockSize), 10d);

        public Color BlockColor
        {
            get { return GetValue(BlockColorProperty); }
            set { SetValue(BlockColorProperty, value); }
        }
        public static readonly StyledProperty<Color> BlockColorProperty = AvaloniaProperty
            .Register<Mesh, Color>(nameof(BlockColor), Colors.DarkGray);

        public double BlockOpacity
        {
            get { return GetValue(BlockOpacityProperty); }
            set { SetValue(BlockOpacityProperty, value); }
        }
        public static readonly StyledProperty<double> BlockOpacityProperty = AvaloniaProperty
            .Register<Mesh, double>(nameof(BlockOpacity), 0.2d, false, BindingMode.Default, null, CoerceBlockOpacity);
        private static double CoerceBlockOpacity(IAvaloniaObject sender, double baseValue)
        {
            return baseValue switch
            {
                > 1 => 1,
                < 0 => 0,
                _   => baseValue
            };
        }

        public bool UseMesh
        {
            get { return GetValue(UseMeshProperty); }
            set { SetValue(UseMeshProperty, value); }
        }
        public static readonly StyledProperty<bool> UseMeshProperty = AvaloniaProperty
            .Register<Mesh, bool>(nameof(UseMesh), true);



        static Mesh()
        {
            AffectsRender<Mesh>(UseMeshProperty);
            AffectsRender<Mesh>(BlockSizeProperty);
            AffectsRender<Mesh>(BlockColorProperty);
            AffectsRender<Mesh>(BlockOpacityProperty);

            BackgroundProperty.Changed.AddClassHandler<Mesh>((grid, args) =>
            {
                if (args.NewValue is SolidColorBrush brush)
                {
                    grid.BlockColor = brush.Color;
                }
            });
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (UseMesh == false)
            {
                return;
            }

            var data = GetPath(BlockSize, this.Bounds.Width, this.Bounds.Height);
            
            context.DrawGeometry(new SolidColorBrush(BlockColor)
            {
                Opacity = BlockOpacity,
            }, null, data);
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
