using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg.Sample
{
    public class CostToBrushCvt : IValueConverter
    {
        public static CostToBrushCvt Cvt { get; } = new CostToBrushCvt();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TimeSpan span)
            {
                if (span.TotalMilliseconds < 1)
                {
                    return Brushes.Green;
                }
                else if (span.TotalMilliseconds < 10)
                {
                    return Brushes.YellowGreen;
                }
                else if (span.TotalMilliseconds < 100)
                {
                    return Brushes.Blue;
                }
                else if (span.TotalMilliseconds < 500)
                {
                    return Brushes.Orange;
                }
                else if (span.TotalMilliseconds < 1000)
                {
                    return Brushes.Red;
                }
            }

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return BindingOperations.DoNothing;
        }
    }
}
