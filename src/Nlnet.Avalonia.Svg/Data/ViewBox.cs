using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Nlnet.Avalonia.Svg.Utils;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Creates an <see cref="ViewBox"/> from a string representation.
    /// </summary>
    public class ViewBoxConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
        {
            return value is string s ? ViewBox.Parse(s) : null;
        }
    }

    [TypeConverter(typeof(ViewBoxConverter))]
    public class ViewBox
    {
        public Point  Origin { get; }
        public double Width  { get; }
        public double Height { get; }

        public ViewBox(Point origin, double width, double height)
        {
            Origin = origin;
            Width  = width;
            Height = height;
        }

        public ViewBox(double x, double y, double width, double height)
        {
            Origin = new Point(x, y);
            Width  = width;
            Height = height;
        }

        public static ViewBox? Parse(string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            var tokenizer = new SafeStringTokenizer(str, ' ', ',');
            if (tokenizer.GetCount() != 4)
            {
                return null;
            }

            var x = double.Parse(tokenizer.Item1);
            var y = double.Parse(tokenizer.Item2);
            var w = double.Parse(tokenizer.Item3);
            var h = double.Parse(tokenizer.Item4);

            return new ViewBox(x, y, w, h);
        }
    }
}
