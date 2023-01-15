using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.Utils;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Base value converters.
    /// </summary>
    internal static class Parsers
    {
        /// <summary>
        /// Try convert value string to <see cref="string"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryToString(this string valueString, out string result)
        {
            result = valueString;
            return true;
        }

        /// <summary>
        /// Convert value string to <see cref="double"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static double ToDouble(this string valueString)
        {
            if (Regex.IsMatch(valueString, "^[0-9.]*%$"))
            {
                return double.Parse(valueString[..^1]) / 100;
            }
            else
            {
                return double.Parse(valueString);
            }
        }

        /// <summary>
        /// Try convert value string to <see cref="double"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryToDouble(this string valueString, out double value)
        {
            try
            {
                value = ToDouble(valueString);
                return true;
            }
            catch
            {
                value = double.NaN;
                return false;
            }
        }

        /// <summary>
        /// Convert value string to <see cref="IBrush"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static IBrush ToIBrush(this string valueString)
        {
            return Brush.Parse(valueString);
        }

        /// <summary>
        /// Try convert value string to <see cref="IBrush"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static bool TryToIBrush(this string valueString, out IBrush? brush)
        {
            try
            {
                brush = ToIBrush(valueString);
                return true;
            }
            catch
            {
                brush = null;
                return false;
            }
        }

        /// <summary>
        /// Convert value string to <see cref="Thickness"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static Thickness ToThickness(this string valueString)
        {
            return Thickness.Parse(valueString);
        }

        /// <summary>
        /// Try convert value string to <see cref="Thickness"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public static bool TryToThickness(this string valueString, out Thickness thickness)
        {
            try
            {
                thickness = ToThickness(valueString);
                return true;
            }
            catch
            {
                thickness = new Thickness(0);
                return false;
            }
        }

        /// <summary>
        /// Convert value string to <see cref="Geometry"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static Geometry ToGeometry(this string valueString)
        {
            return Geometry.Parse(valueString);
        }

        /// <summary>
        /// Try convert value string to <see cref="Geometry"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static bool TryToGeometry(this string valueString, out Geometry? geometry)
        {
            try
            {
                geometry = ToGeometry(valueString);
                return true;
            }
            catch
            {
                geometry = null;
                return false;
            }
        }

        /// <summary>
        /// Convert value string to <see cref="PointList"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static PointList ToPointList(this string valueString)
        {
            var results   = new PointList();
            var tokenizer = new SafeStringTokenizer(valueString, StringSplitOptions.RemoveEmptyEntries, ' ', ',');
            var values = tokenizer
                .GetTokens()
                .Select(t =>
                {
                    if (double.TryParse(t, out var dValue))
                    {
                        return (double?) dValue;
                    }
                    return null;
                })
                .OfType<double>()
                .ToList();

            for (var i = 0; i < values.Count; i+=2)
            {
                if (i + 1 >= values.Count)
                {
                    break;
                }
                var x = values[i];
                var y = values[i + 1];
                results.Add(new Point(x, y));
            }

            return results;
        }

        /// <summary>
        /// Try convert value string to <see cref="PointList"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool TryToPointList(this string valueString, out PointList points)
        {
            try
            {
                points = ToPointList(valueString);
                return true;
            }
            catch
            {
                points = new PointList();
                return false;
            }
        }
    }
}
