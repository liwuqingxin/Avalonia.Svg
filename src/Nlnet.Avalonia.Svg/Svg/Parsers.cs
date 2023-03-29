using System;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.Utils;

namespace Nlnet.Avalonia.Svg
{
    // TODO 拆分成类。
    /// <summary>
    /// Base value converters.
    /// </summary>
    internal static class Parsers
    {
        public static bool TryToString(this string valueString, out string result)
        {
            result = valueString;
            return true;
        }

        public static double ToDouble(this string valueString)
        {
            if (Regex.IsMatch(valueString, "^[-0-9.]*%$"))
            {
                return double.Parse(valueString[..^1]) / 100;
            }
            else if (Regex.IsMatch(valueString, "^[-0-9.]*px$"))
            {
                return double.Parse(valueString[..^2]);
            }
            else
            {
                return double.Parse(valueString);
            }
        }

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

        public static LightBrush ToLightBrush(this string valueString)
        {
            if (valueString == "none")
            {
                return new LightSolidColorBrush(Colors.Transparent);
            }

            return new LightSolidColorBrush(Color.Parse(valueString));
        }

        public static bool TryToLightBrush(this string valueString, out LightBrush? brush)
        {
            try
            {
                brush = ToLightBrush(valueString);
                return true;
            }
            catch
            {
                brush = null;
                return false;
            }
        }

        public static Thickness ToThickness(this string valueString)
        {
            return Thickness.Parse(valueString);
        }

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

        public static Geometry ToGeometry(this string valueString)
        {
            return PathGeometry.Parse(valueString);
        }

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

        public static Transform ToTransform(this string valueString)
        {
            //TODO 支持空格间隔
            var regex = new Regex("(translate\\(.*?\\s*?[,\\s]\\s*?.*?\\))|(scale\\(.*?\\s*?[,\\s]\\s*?.*?\\))|(rotate\\(.*?\\))|(matrix\\(.*?\\))");
            var matches = regex.Matches(valueString);

            var transform = new TransformGroup();
            foreach (Match match in matches)
            {
                if (match.Value.StartsWith("translate"))
                {
                    var translateStrings = match.Value[10..^1].Split(new char[]{ ',' , ' '}, StringSplitOptions.RemoveEmptyEntries);
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
                    var scaleStrings = match.Value[6..^1].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
                else if (match.Value.StartsWith("matrix"))
                {
                    var matrixStrings = match.Value[7..^1].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (matrixStrings.Length != 6)
                    {
                        continue;
                    }

                    var a = double.Parse(matrixStrings[0]);
                    var b = double.Parse(matrixStrings[1]);
                    var c = double.Parse(matrixStrings[2]);
                    var d = double.Parse(matrixStrings[3]);
                    var e = double.Parse(matrixStrings[4]);
                    var f = double.Parse(matrixStrings[5]);

                    transform.Children.Add(new MatrixTransform(new Matrix(a, b, c, d, e, f)));
                }
            }

            return transform;
        }

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

        public static bool TryToFillRule(this string valueString, out FillRule fillRule)
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

        public static PenLineCap ToPenLineCap(this string valueString)
        {
            if (string.Equals(valueString, "butt", StringComparison.CurrentCultureIgnoreCase))
            {
                return PenLineCap.Flat;
            }
            else if (string.Equals(valueString, PenLineCap.Square.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return PenLineCap.Square;
            }
            else if (string.Equals(valueString, PenLineCap.Round.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return PenLineCap.Round;
            }

            return PenLineCap.Flat;
        }

        public static bool TryToPenLineCap(this string valueString, out PenLineCap penLineCap)
        {
            if (string.Equals(valueString, "butt", StringComparison.CurrentCultureIgnoreCase))
            {
                penLineCap = PenLineCap.Flat;
                return true;
            }
            else if (string.Equals(valueString, PenLineCap.Square.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                penLineCap = PenLineCap.Square;
                return true;
            }
            else if (string.Equals(valueString, PenLineCap.Round.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                penLineCap = PenLineCap.Round;
                return true;
            }

            penLineCap = PenLineCap.Flat;
            return false;
        }

        public static PenLineJoin ToPenLineJoin(this string valueString)
        {
            if (string.Equals(valueString, PenLineJoin.Bevel.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return PenLineJoin.Bevel;
            }
            else if (string.Equals(valueString, PenLineJoin.Miter.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return PenLineJoin.Miter;
            }
            else if (string.Equals(valueString, PenLineJoin.Round.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return PenLineJoin.Round;
            }
            else if (string.Equals(valueString, "miter-clip") || string.Equals(valueString, "arcs"))
            {
                return PenLineJoin.Miter;
            }

            return PenLineJoin.Miter;
        }

        public static bool TryToPenLineJoin(this string valueString, out PenLineJoin penLineJoin)
        {
            if (string.Equals(valueString, PenLineJoin.Bevel.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                penLineJoin = PenLineJoin.Bevel;
                return true;
            }
            else if (string.Equals(valueString, PenLineJoin.Miter.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                penLineJoin = PenLineJoin.Miter;
                return true;
            }
            else if (string.Equals(valueString, PenLineJoin.Round.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                penLineJoin = PenLineJoin.Round;
                return true;
            }
            else if (string.Equals(valueString, "miter-clip") || string.Equals(valueString, "arcs"))
            {
                penLineJoin = PenLineJoin.Miter;
                return true;
            }

            penLineJoin = PenLineJoin.Miter;
            return false;
        }

        public static DoubleList ToDoubleList(this string valueString)
        {
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
                .OfType<double>();

            return new DoubleList(values);
        }

        public static bool TryToDoubleList(this string valueString, out DoubleList doubleList)
        {
            try
            {
                doubleList = ToDoubleList(valueString);
                return true;
            }
            catch
            {
                doubleList = new DoubleList();
                return false;
            }
        }

        public static GradientSpreadMethod ToGradientSpreadMethod(this string valueString)
        {
            if (string.Equals(valueString, GradientSpreadMethod.Pad.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return GradientSpreadMethod.Pad;
            }
            else if (string.Equals(valueString, GradientSpreadMethod.Reflect.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return GradientSpreadMethod.Reflect;
            }
            else if (string.Equals(valueString, GradientSpreadMethod.Repeat.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return GradientSpreadMethod.Repeat;
            }

            return GradientSpreadMethod.Pad;
        }

        public static bool TryToGradientSpreadMethod(this string valueString, out GradientSpreadMethod penLineJoin)
        {
            if (string.Equals(valueString, GradientSpreadMethod.Pad.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                penLineJoin = GradientSpreadMethod.Pad;
                return true;
            }
            else if (string.Equals(valueString, GradientSpreadMethod.Reflect.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                penLineJoin = GradientSpreadMethod.Reflect;
                return true;
            }
            else if (string.Equals(valueString, GradientSpreadMethod.Repeat.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                penLineJoin = GradientSpreadMethod.Repeat;
                return true;
            }

            penLineJoin = GradientSpreadMethod.Pad;
            return false;
        }

        public static SvgUnit ToSvgUnit(this string valueString)
        {
            return Enum.Parse<SvgUnit>(valueString);
        }

        public static bool TryToSvgUnit(this string valueString, out SvgUnit gradientUnit)
        {
            return Enum.TryParse<SvgUnit>(valueString, out gradientUnit);
        }

        public static SvgMarkerUnits ToSvgMarkerUnits(this string valueString)
        {
            return Enum.Parse<SvgMarkerUnits>(valueString);
        }

        public static bool TryToSvgMarkerUnits(this string valueString, out SvgMarkerUnits gradientUnit)
        {
            return Enum.TryParse<SvgMarkerUnits>(valueString, out gradientUnit);
        }

        public static SvgMarkerOrient ToSvgMarkerOrient(this string valueString)
        {
            if (valueString == SvgMarkerOrientMode.auto.ToString())
            {
                return SvgMarkerOrient.Default;
            }
            else if (valueString.Replace("-", "_") == SvgMarkerOrientMode.auto_start_reverse.ToString())
            {
                return new SvgMarkerOrient()
                {
                    Mode = SvgMarkerOrientMode.auto_start_reverse,
                };
            }
            else
            {
                //
                // Unit：
                // https://www.w3.org/TR/SVG2/painting.html#OrientAttribute
                // https://www.w3.org/TR/2012/WD-css3-values-20120308/#angles
                //
                var match = new Regex("^(.*?)(deg|grad|rad|turn|)$").Match(valueString);
                if (match.Success)
                {
                    var value = match.Groups[1].Value;
                    if (double.TryParse(value, out var angle))
                    {
                        if (match.Groups.Count > 2)
                        {
                            var unit = match.Groups[2].Value;
                            switch (unit)
                            {
                                case "deg":
                                    angle = angle / 180 * Math.PI;
                                    break;
                                case "grad":
                                    angle = angle / 400 * 2 * Math.PI ;
                                    break;
                                case "turn":
                                    angle = angle * 2 * Math.PI;
                                    break;
                                case "rad":
                                    break;
                                case "":
                                    break;
                                default:
                                    break;
                            }
                        }

                        return new SvgMarkerOrient()
                        {
                            Mode  = SvgMarkerOrientMode.angle,
                            Angle = angle,
                        };
                    }
                }

                return SvgMarkerOrient.Default;
            }
        }

        public static bool TryToSvgMarkerOrient(this string valueString, out SvgMarkerOrient orient)
        {
            try
            {
                orient = ToSvgMarkerOrient(valueString);
                return true;
            }
            catch
            {
                orient = SvgMarkerOrient.Default;
                return false;
            }
       
        }

        public static RefX ToRefX(this string valueString)
        {
            return Enum.Parse<RefX>(valueString.Replace("-", "_"));
        }

        public static bool TryToRefX(this string valueString, out RefX gradientUnit)
        {
            return Enum.TryParse<RefX>(valueString.Replace("-", "_"), out gradientUnit);
        }

        public static RefY ToRefY(this string valueString)
        {
            return Enum.Parse<RefY>(valueString.Replace("-", "_"));
        }

        public static bool TryToRefY(this string valueString, out RefY gradientUnit)
        {
            return Enum.TryParse<RefY>(valueString.Replace("-", "_"), out gradientUnit);
        }

        public static ViewBox? ToViewBox(this string valueString)
        {
            if (valueString == "none")
            {
                // TODO none 的场景
            }

            return ViewBox.Parse(valueString);
        }

        public static bool TryToViewBox(this string valueString, out ViewBox? viewBox)
        {
            try
            {
                viewBox = ToViewBox(valueString);
                return true;
            }
            catch
            {
                viewBox = null;
                return false;
            }
        }

        public static PreserveAspectRatio? ToPreserveAspectRatio(this string valueString)
        {
            return PreserveAspectRatio.Parse(valueString);
        }

        public static bool TryToPreserveAspectRatio(this string valueString, out PreserveAspectRatio? preserveAspectRatio)
        {
            try
            {
                preserveAspectRatio = ToPreserveAspectRatio(valueString);
                return true;
            }
            catch
            {
                preserveAspectRatio = null;
                return false;
            }
        }

        public static ISvgStyle? ToISvgStyle(this string valueString)
        {
            return SvgStyleImpl.Parse(null, valueString);
        }

        public static bool TryToISvgStyle(this string valueString, out ISvgStyle? svgStyle)
        {
            try
            {
                svgStyle = ToISvgStyle(valueString);
                return true;
            }
            catch
            {
                svgStyle = null;
                return false;
            }
        }
    }
}
