using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Svg.Utils;

namespace Avalonia.Svg.Controls
{
    /// <summary>
    /// Icon element. It supports iconfont, geometry and image.
    /// </summary>
    public class Icon : Control
    {
        #region Attached Properties

        public static string? GetIcon(Visual host)
        {
            return host.GetValue(IconProperty);
        }
        public static void SetIcon(Visual host, string? value)
        {
            host.SetValue(IconProperty, value);
        }
        public static readonly AttachedProperty<string?> IconProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, string?>("Icon");

        public static FontFamily? GetIconFamily(Visual host)
        {
            return host.GetValue(IconFamilyProperty);
        }
        public static void SetIconFamily(Visual host, FontFamily? value)
        {
            host.SetValue(IconFamilyProperty, value);
        }
        public static readonly AttachedProperty<FontFamily?> IconFamilyProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, FontFamily?>("IconFamily", null, true);

        public static double GetIconSize(Visual host)
        {
            return host.GetValue(IconSizeProperty);
        }
        public static void SetIconSize(Visual host, double value)
        {
            host.SetValue(IconSizeProperty, value);
        }
        public static readonly AttachedProperty<double> IconSizeProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, double>("IconSize", 16d);

        public static IBrush? GetIconBrush(Visual host)
        {
            return host.GetValue(IconBrushProperty);
        }
        public static void SetIconBrush(Visual host, IBrush? value)
        {
            host.SetValue(IconBrushProperty, value);
        }
        public static readonly AttachedProperty<IBrush?> IconBrushProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, IBrush?>("IconBrush", Brushes.Black);

        public static Stretch GetIconStretch(Visual host)
        {
            return host.GetValue(IconStretchProperty);
        }
        public static void SetIconStretch(Visual host, Stretch value)
        {
            host.SetValue(IconStretchProperty, value);
        }
        public static readonly AttachedProperty<Stretch> IconStretchProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, Stretch>("IconStretch", Stretch.Uniform);

        public static Geometry? GetIconData(Visual host)
        {
            return host.GetValue(IconDataProperty);
        }
        public static void SetIconData(Visual host, Geometry? value)
        {
            host.SetValue(IconDataProperty, value);
        }
        public static readonly AttachedProperty<Geometry?> IconDataProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, Geometry?>("IconData");

        public static IImage? GetIconImage(Visual host)
        {
            return host.GetValue(IconImageProperty);
        }
        public static void SetIconImage(Visual host, IImage? value)
        {
            host.SetValue(IconImageProperty, value);
        }
        public static readonly AttachedProperty<IImage?> IconImageProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, IImage?>("IconImage");

        public static string? GetIconSvgData(Visual host)
        {
            return host.GetValue(IconSvgDataProperty);
        }
        public static void SetIconSvgData(Visual host, string? value)
        {
            host.SetValue(IconSvgDataProperty, value);
        }
        public static readonly AttachedProperty<string?> IconSvgDataProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, string?>("IconSvgData");

        public static string? GetCheckedIcon(Visual host)
        {
            return host.GetValue(CheckedIconProperty);
        }
        public static void SetCheckedIcon(Visual host, string? value)
        {
            host.SetValue(CheckedIconProperty, value);
        }
        public static readonly AttachedProperty<string?> CheckedIconProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, string?>("CheckedIcon");

        public static Geometry? GetCheckedIconData(Visual host)
        {
            return host.GetValue(CheckedIconDataProperty);
        }
        public static void SetCheckedIconData(Visual host, Geometry? value)
        {
            host.SetValue(CheckedIconDataProperty, value);
        }
        public static readonly AttachedProperty<Geometry?> CheckedIconDataProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, Geometry?>("CheckedIconData");

        public static IImage? GetCheckedIconImage(Visual host)
        {
            return host.GetValue(CheckedIconImageProperty);
        }
        public static void SetCheckedIconImage(Visual host, IImage? value)
        {
            host.SetValue(CheckedIconImageProperty, value);
        }
        public static readonly AttachedProperty<IImage?> CheckedIconImageProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, IImage?>("CheckedIconImage");

        #endregion



        #region Dependency Property

        public static bool GetShowRenderBounds(Visual host)
        {
            return host.GetValue(ShowRenderBoundsProperty);
        }
        public static void SetShowRenderBounds(Visual host, bool value)
        {
            host.SetValue(ShowRenderBoundsProperty, value);
        }
        public static readonly AttachedProperty<bool> ShowRenderBoundsProperty = AvaloniaProperty
            .RegisterAttached<Icon, Visual, bool>("ShowRenderBounds", false, true);

        #endregion



        #region Fields

        private FormattedText? _formattedText;

        #endregion



        #region ctor

        static Icon()
        {
            AffectsRender<Icon>(
                IconProperty, 
                IconFamilyProperty, 
                IconSizeProperty, 
                IconBrushProperty, 
                IconStretchProperty, 
                IconDataProperty, 
                IconImageProperty, 
                IconSvgDataProperty,
                CheckedIconProperty, 
                CheckedIconDataProperty, 
                CheckedIconImageProperty);
            AffectsMeasure<Icon>(
                IconProperty,
                IconFamilyProperty,
                IconSizeProperty,
                IconStretchProperty,
                IconSvgDataProperty,
                CheckedIconProperty);
            AffectsArrange<Icon>(
                IconProperty,
                IconFamilyProperty,
                IconSizeProperty,
                IconStretchProperty,
                IconSvgDataProperty,
                CheckedIconProperty);

            IconProperty.Changed.AddClassHandler<Icon>((sender, e) =>
            {
                sender.EnsureRenderedFormattedText();
                sender.UpdateVisibility();
            });
            IconFamilyProperty.Changed.AddClassHandler<Icon>((sender, e) =>
            {
                if (sender._formattedText != null && e.NewValue is FontFamily fontFamily)
                {
                    sender._formattedText.SetFontFamily(fontFamily);
                }
            });
            IconSizeProperty.Changed.AddClassHandler<Icon>((sender, e) =>
            {
                if (sender._formattedText != null && e.NewValue is double iconSize)
                {
                    sender._formattedText.SetFontSize(iconSize);
                }
            });
            IconBrushProperty.Changed.AddClassHandler<Icon>((sender, e) =>
            {
                if (sender._formattedText != null && e.NewValue is IBrush brush)
                {
                    sender._formattedText.SetForegroundBrush(brush);
                }
            });
            IconDataProperty.Changed.AddClassHandler<Icon>((sender, e) =>
            {
                sender.EnsureRenderedGeometry(true);
                sender.UpdateVisibility();
            });
            IconImageProperty.Changed.AddClassHandler<Icon>((sender, e) =>
            {
                sender.UpdateVisibility();
            });
            ShowRenderBoundsProperty.Changed.AddClassHandler<Icon>((icon, args) =>
            {
                icon.InvalidateVisual();
            });
            IconSvgDataProperty.Changed.AddClassHandler<Icon>((icon, args) =>
            {
                var data = Icon.GetIconSvgData(icon);
                icon._svg = string.IsNullOrWhiteSpace(data) ? null : SvgTagFactory.LoadSvg(data);

                icon.UpdateVisibility();
                icon.InvalidateVisual();
            });
        }

        private void EnsureRenderedFormattedText()
        {
            var icon = GetIcon(this);
            if (string.IsNullOrEmpty(icon))
            {
                _formattedText = null;
                return;
            }

            var size       = GetIconSize(this);
            var brush      = GetIconBrush(this) ?? Brushes.Black;
            var iconFamily = GetIconFamily(this) ?? FontFamily.Default;
            var typeface   = new Typeface(iconFamily);

            _formattedText = new FormattedText(icon, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, size, brush);
        }

        private void UpdateVisibility()
        {
            IsVisible = Icon.GetIcon(this) != null || Icon.GetIconData(this) != null || Icon.GetIconImage(this) != null || Icon.GetIconSvgData(this) != null;
        }

        public Icon()
        {
            IsVisible = false;
            VerticalAlignment = VerticalAlignment.Center;
        }

        #endregion



        #region Geometry

        private class BoxedMatrix
        {
            public readonly Matrix Value;

            public BoxedMatrix(Matrix value) => Value = value;
        }

        // TODO To use map saving memory, see https://stackoverflow.com/questions/18153065/the-use-of-uncommonfieldt-in-wpf/18280136#18280136
        private BoxedMatrix? _boxedMatrix;
        private Geometry? _renderedGeometry = null;

        public Geometry? RenderedGeometry
        {
            get
            {
                EnsureRenderedGeometry(false);
                return _renderedGeometry;
            }
        }

        public Transform GeometryTransform
        {
            get
            {
                var boxedMatrix = _boxedMatrix;
                return boxedMatrix == null ? new TranslateTransform() : new MatrixTransform(boxedMatrix.Value);
            }
        }

        private void EnsureRenderedGeometry(bool repaint)
        {
            if (_renderedGeometry != null && repaint == false)
            {
                return;
            }
            _renderedGeometry = GetIconData(this);
            if (_renderedGeometry == null)
            {
                return;
            }
            if (GetIconStretch(this) == Stretch.None)
            {
                return;
            }

            _renderedGeometry = _renderedGeometry.Clone();

            var transform = _renderedGeometry.Transform;
            var matrix    = _boxedMatrix?.Value ?? Matrix.Identity;

            if (transform == null || transform.Value.IsIdentity)
            {
                _renderedGeometry.Transform = new MatrixTransform(matrix);
            }
            else
            {
                _renderedGeometry.Transform = new MatrixTransform(transform.Value * matrix);
            }
        }

        private Size GetStretchedRenderSize(Stretch mode, double strokeThickness, Size availableSize, Rect geometryBounds)
        {
            GetStretchMetrics(mode, strokeThickness, availableSize, geometryBounds, out _, out _, out _, out _, out var stretchedSize);
            return stretchedSize;
        }

        private Size GetStretchedRenderSizeAndSetStretchMatrix(Stretch mode, double strokeThickness, Size availableSize, Rect geometryBounds)
        {
            GetStretchMetrics(mode, strokeThickness, availableSize, geometryBounds, out var xScale, out var yScale, out var dX, out var dY, out var stretchedSize);

            var identity = Matrix.Identity;
            var x        = geometryBounds.X;
            var y        = geometryBounds.Y;

            identity.ScaleAt(xScale, yScale, x, y);
            identity.Translate(dX, dY);

            _boxedMatrix = new BoxedMatrix(identity);
            ResetRenderedGeometry();

            return stretchedSize;
        }

        private void ResetRenderedGeometry()
        {
            _renderedGeometry = null;
        }

        private static void GetStretchMetrics(Stretch mode, double strokeThickness, Size availableSize, Rect geometryBounds, out double xScale, out double yScale, out double dX, out double dY, out Size stretchedSize)
        {
            if (!geometryBounds.IsEmpty)
            {
                var num = strokeThickness / 2.0;
                var flag = false;
                xScale = Math.Max(availableSize.Width - strokeThickness, 0.0);
                yScale = Math.Max(availableSize.Height - strokeThickness, 0.0);
                dX = num - geometryBounds.Left;
                dY = num - geometryBounds.Top;
                if (geometryBounds.Width > xScale * double.Epsilon)
                {
                    xScale /= geometryBounds.Width;
                }
                else
                {
                    xScale = 1.0;
                    if (geometryBounds.Width == 0.0)
                        flag = true;
                }
                if (geometryBounds.Height > yScale * double.Epsilon)
                {
                    yScale /= geometryBounds.Height;
                }
                else
                {
                    yScale = 1.0;
                    if (geometryBounds.Height == 0.0)
                        flag = true;
                }
                if (mode != Stretch.Fill && !flag)
                {
                    if (mode == Stretch.Uniform)
                    {
                        if (yScale > xScale)
                            yScale = xScale;
                        else
                            xScale = yScale;
                    }
                    else if (xScale > yScale)
                        yScale = xScale;
                    else
                        xScale = yScale;
                }
                stretchedSize = new Size(geometryBounds.Width * xScale + strokeThickness, geometryBounds.Height * yScale + strokeThickness);
            }
            else
            {
                xScale = yScale = 1.0;
                dX = dY = 0.0;
                stretchedSize = new Size(0.0, 0.0);
            }
        }

        private static bool SizeIsInvalidOrEmpty(Size size)
        {
            return double.IsNaN(size.Width) || double.IsNaN(size.Height) || size.IsDefault;
        }

        private Size GetNaturalSize()
        {
            var geometry = GetIconData(this);
            if (geometry == null)
            {
                return Size.Empty;
            }

            var renderBounds = geometry.GetRenderBounds(null);

            return renderBounds.Size;
        }

        private Rect GetDefiningGeometryBounds()
        {
            var geometry = GetIconData(this);
            if (geometry == null)
            {
                return Rect.Empty;
            }

            return geometry.Bounds;
        }

        #endregion



        #region Image

        private Size GetImageStretchedSize()
        {
            var stretch = GetIconStretch(this);
            var image   = GetIconImage(this);
            var size    = GetIconSize(this);
            if (image == null)
            {
                return Size.Empty;
            }

            switch (stretch)
            {
                case Stretch.None:
                    return new Size(image.Size.Width, image.Size.Height);
                case Stretch.Fill:
                    return new Size(size, size);
                case Stretch.Uniform:
                {
                    var xr = image.Size.Width  / size;
                    var yr = image.Size.Height / size;
                    var r  = Math.Max(xr, yr);
                    return new Size(image.Size.Width / r, image.Size.Height / r);
                }
                case Stretch.UniformToFill:
                {
                    var xr = image.Size.Width  / size;
                    var yr = image.Size.Height / size;
                    var r  = Math.Min(xr, yr);
                    return new Size(image.Size.Width / r, image.Size.Height / r);
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion



        #region Svg

        private ISvg? _svg;

        #endregion



        #region Render & Layout

        public override void Render(DrawingContext drawingContext)
        {
            base.Render(drawingContext);

            var brush = GetIconBrush(this);

            // Border
            if (GetShowRenderBounds(this))
            {
                var formattedText = new FormattedText(
                    $"({this.Bounds.Width:#.##}, {this.Bounds.Height:#.##})",
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(FontFamily.Default),
                    8,
                    Brushes.Red);
                drawingContext.DrawText(formattedText, new Point(0, -12));

                var pen = new Pen(Brushes.Red, 0.5)
                {
                    DashStyle = new DashStyle(new double[] { 5, 5 }, 0)
                };
                drawingContext.DrawRectangle(null, pen, new Rect(new Size(this.Bounds.Width, this.Bounds.Height)));
            }

            // Image
            var image = GetIconImage(this);
            if (image != null)
            {
                if (GetIsTrulyEnabled())
                {
                    drawingContext.DrawImage(image, new Rect(GetImageStretchedSize()));
                }
                else
                {
                    using (drawingContext.PushOpacity(0.3))
                    {
                        drawingContext.DrawImage(image, new Rect(GetImageStretchedSize()));
                    }
                }
            }

            // Geometry
            EnsureRenderedGeometry(false);
            if (_renderedGeometry != null)
            {
                drawingContext.DrawGeometry(brush, null, _renderedGeometry);
            }

            // Iconfont
            if (_formattedText != null)
            {
                // The height of FormattedText for icon font has min value of about 22 which should be a bug.
                // So we just set the size as width * width.
                // And, we must offset it along y-axis because the icon stays at bottom in it's bounds.
                var dValue = ((_formattedText.Baseline - _formattedText.Width) + (_formattedText.Extent - _formattedText.Width)) / 2;
                if (dValue > 0)
                {
                    dValue = -dValue;
                }
                else
                {
                    dValue = 0;
                }
                drawingContext.DrawText(_formattedText, new Point(0, dValue));
            }

            // Svg
            if (_svg != null)
            {
                _svg.Render(drawingContext);
            }
        }

        private bool GetIsTrulyEnabled()
        {
            // In avalonia 11.0.0-preview4, the IsEnabled property is not inherited from ancestors even the control is truly disabled.
            
            IControl? selfOrAncestor = this;
            while (true)
            {
                if (selfOrAncestor == null)
                {
                    return true;
                }
                if (selfOrAncestor.IsEnabled == false)
                {
                    return false;
                }
                selfOrAncestor = selfOrAncestor.Parent;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size0 = MeasureIconImage(availableSize);
            var size1 = MeasureIconfont(availableSize);
            var size2 = MeasureGeometry(availableSize);
            var size3 = MeasureSvg(availableSize);

            var w = Math.Max(size0.Width, size1.Width);
            w = Math.Max(size2.Width, w);
            w = Math.Max(size3.Width, w);
            var h = Math.Max(size0.Height, size1.Height);
            h = Math.Max(size2.Height, h);
            h = Math.Max(size3.Height, h);

            return new Size(w, h);
        }

        private Size MeasureIconImage(Size availableSize)
        {
            if (GetIconImage(this) == null)
            {
                return new Size(0d, 0d);
            }

            return GetImageStretchedSize();
        }

        private Size MeasureIconfont(Size availableSize)
        {
            if (_formattedText == null)
            {
                return new Size(0d, 0d);
            }

            // The height of FormattedText for icon font has min value of about 22 which should be a bug.
            // So we just set the size as width * width.
            return new Size(_formattedText.Width, _formattedText.Width);
        }

        private Size MeasureGeometry(Size availableSize)
        {
            var stretch  = GetIconStretch(this);
            var iconSize = GetIconSize(this);
            var size = stretch != Stretch.None
                ? GetStretchedRenderSize(stretch, 0, new Size(iconSize, iconSize), GetDefiningGeometryBounds())
                : GetNaturalSize();

            if (SizeIsInvalidOrEmpty(size))
            {
                size = new Size(0d, 0d);
                _renderedGeometry = null;
            }

            return size;
        }

        private Size MeasureSvg(Size availableSize)
        {
            if (_svg == null)
            {
                return Size.Empty;
            }
            return _svg.GetRenderSize();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size0 = ArrangeIconImage(finalSize);
            var size1 = ArrangeIconfont(finalSize);
            var size2 = ArrangeGeometry(finalSize);
            var size3 = ArrangeSvg(finalSize);

            var w = Math.Max(size0.Width, size1.Width);
            w = Math.Max(size2.Width, w);
            w = Math.Max(size3.Width, w);
            var h = Math.Max(size0.Height, size1.Height);
            h = Math.Max(size2.Height, h);
            h = Math.Max(size3.Height, h);

            return new Size(w, h);
        }

        private Size ArrangeIconImage(Size finalSize)
        {
            if (GetIconImage(this) == null)
            {
                return new Size(0d, 0d);
            }

            return GetImageStretchedSize();
        }

        private Size ArrangeIconfont(Size finalSize)
        {
            if (_formattedText == null)
            {
                return new Size(0d, 0d);
            }

            // The height of FormattedText for icon font has min value of about 22 which should be a bug.
            // So we just set the size as width * width.
            return new Size(_formattedText.Width, _formattedText.Width);
        }

        private Size ArrangeGeometry(Size finalSize)
        {
            if (GetIconData(this) == null)
            {
                return new Size(0d, 0d);
            }

            var  stretch  = GetIconStretch(this);
            var  iconSize = GetIconSize(this);
            Size size;
            if (stretch == Stretch.None)
            {
                _boxedMatrix = null;
                ResetRenderedGeometry();
                size = finalSize;
            }
            else
            {
                size = GetStretchedRenderSizeAndSetStretchMatrix(stretch, 0, new Size(iconSize, iconSize), GetDefiningGeometryBounds());
            }

            if (SizeIsInvalidOrEmpty(size))
            {
                size = finalSize;
                _renderedGeometry = null;
            }

            return size;
        }

        private Size ArrangeSvg(Size finalSize)
        {
            if (_svg == null)
            {
                return Size.Empty;
            }
            return _svg.GetRenderSize();
        }

        #endregion
    }
}
