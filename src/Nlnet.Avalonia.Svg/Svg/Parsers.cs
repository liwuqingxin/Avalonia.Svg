using System;
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
        /// Try to convert value string to <see cref="string"/>.
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
            if (Regex.IsMatch(valueString, "^[-0-9.]*%$"))
            {
                return double.Parse(valueString[..^1]) / 100;
            }
            else
            {
                return double.Parse(valueString);
            }
        }

        /// <summary>
        /// Try to convert value string to <see cref="double"/>.
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
            if (valueString == "none")
            {
                return Brushes.Transparent;
            }

            return Brush.Parse(valueString);
        }

        /// <summary>
        /// Try to convert value string to <see cref="IBrush"/>.
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
        /// Try to convert value string to <see cref="Thickness"/>.
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
        /// Try to convert value string to <see cref="Geometry"/>.
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

            for (var i = 0; i < values.Count; i += 2)
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
        /// Try to convert value string to <see cref="PointList"/>.
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

        /// <summary>
        /// Convert value string to <see cref="Transform"/>.<br/>
        /// For example 'translate(336.000000, 336.000000)'.<br/>
        /// For example 2 'translate(66.372939, 117.459729) rotate(16.000000) scale(-66.372939, -117.459729)'.
        /// </summary>
        /// <remarks>Note that the coordinate system is also affected by the transform like rotate or scale, witch is svg behavior.</remarks>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static Transform ToTransform(this string valueString)
        {
            var regex = new Regex("(translate\\(.*?\\s*?,\\s*?.*?\\))|(scale\\(.*?\\s*?,\\s*?.*?\\))|(rotate\\(.*?\\))");
            var matches = regex.Matches(valueString);

            var transform = new TransformGroup();
            foreach (Match match in matches)
            {
                if (match.Value.StartsWith("translate"))
                {
                    var translateStrings = match.Value[10..^1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (translateStrings.Length != 2)
                    {
                        continue;
                    }
                    var x = double.Parse(translateStrings[0]);
                    var y = double.Parse(translateStrings[1]);

                    // Coordinate system of Avalonia is not affected by rotation and scaling, but svg's does.
                    // So the translate transform should be inserted before all existed transforms.
                    transform.Children.Insert(0, new TranslateTransform(x, y));
                }
                else if (match.Value.StartsWith("rotate"))
                {
                    var rotateStrings = match.Value[7..^1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var angle         = double.Parse(rotateStrings[0]);

                    // Default center point is the offset point of transform.
                    var centerX = transform.Value.M31;
                    var centerY = transform.Value.M32;
                    if (rotateStrings.Length > 1)
                    {
                        centerX += double.Parse(rotateStrings[1]);
                    }
                    if (rotateStrings.Length > 2)
                    {
                        centerY += double.Parse(rotateStrings[2]);
                    }

                    // If scale transform changed the face of the geometry, reverse rotation.
                    if (transform.Value.M11 * transform.Value.M22 < 0)
                    {
                        angle = -angle;
                    }
                    transform.Children.Add(new RotateTransform(angle, centerX, centerY));
                }
                else if (match.Value.StartsWith("scale"))
                {
                    var scaleStrings = match.Value[6..^1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (scaleStrings.Length != 2)
                    {
                        continue;
                    }

                    var centerX = transform.Value.M31;
                    var centerY = transform.Value.M32;

                    // Move to the (0,0) to perform scaling.
                    transform.Children.Add(new TranslateTransform(-centerX, -centerY));
                    {
                        var x = double.Parse(scaleStrings[0]);
                        var y = double.Parse(scaleStrings[1]);
                        transform.Children.Add(new ScaleTransform(x, y));
                    }
                    // Restore the translate transform.
                    transform.Children.Add(new TranslateTransform(centerX, centerY));
                }
            }

            return transform;
        }

        /// <summary>
        /// Try to convert value string to <see cref="Transform"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static bool TryToTransform(this string valueString, out Transform? transform)
        {
            try
            {
                transform = ToTransform(valueString);
                return true;
            }
            catch
            {
                transform = null;
                return false;
            }
        }

        /// <summary>
        /// Convert value string to <see cref="FillRule"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        public static FillRule ToFillRule(this string valueString)
        {
            if (string.Equals(valueString, FillRule.NonZero.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return FillRule.NonZero;
            }
            else if (string.Equals(valueString, FillRule.EvenOdd.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return FillRule.EvenOdd;
            }

            return FillRule.NonZero;
        }

        /// <summary>
        /// Try to convert value string to <see cref="FillRule"/>.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="fillRule"></param>
        /// <returns></returns>
        public static bool TryToFillRule(this string valueString, out FillRule? fillRule)
        {
            if (string.Equals(valueString, FillRule.NonZero.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                fillRule = FillRule.NonZero;
                return true;
            }
            else if (string.Equals(valueString, FillRule.EvenOdd.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                fillRule = FillRule.EvenOdd;
                return true;
            }

            fillRule = FillRule.NonZero;
            return false;
        }
    }
}
