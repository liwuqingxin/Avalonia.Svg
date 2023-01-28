using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
// ReSharper disable InconsistentNaming

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// All svg property types.
    /// </summary>
    public static class SvgTypes
    {
        public const string Double    = "double";
        public const string Bool      = "bool";
        public const string Int       = "int";
        public const string String    = "string";
        public const string Brush     = nameof(global::Avalonia.Media.IBrush);
        public const string Thickness = nameof(global::Avalonia.Thickness);
        public const string Geometry  = nameof(global::Avalonia.Media.Geometry);
        public const string PointList = nameof(global::Nlnet.Avalonia.Svg.PointList);
        public const string Transform = nameof(global::Avalonia.Media.Transform);
    }

    /// <summary>
    /// Svg property default value strings.
    /// </summary>
    public static class SvgDefaultValues
    {
        public const string Null = "null";
        public const string Zero = "0d";
        public const string BrushBlack = "Brushes.Black";
    }

    /// <summary>
    /// All svg property definitions.
    /// </summary>
    public static class SvgProperties
    {
        public const string Href        = "xlink:href";
        public const string Class       = "class";
        public const string Id          = "id";
        public const string Version     = "version";
        public const string ViewBox     = "viewBox";
        public const string Style       = "style";
        public const string Type        = "type";
        public const string Data        = "d";
        public const string Offset      = "offset";
        public const string Opacity     = "opacity";
        public const string Points      = "points";
        public const string Fill        = "fill";
        public const string Stroke      = "stroke";
        public const string StrokeWidth = "stroke-width";
        public const string StopOpacity = "stop-opacity";
        public const string StopColor   = "stop-color";
        public const string CX          = "cx";
        public const string CY          = "cy";
        public const string RX          = "rx";
        public const string RY          = "ry";
        public const string X           = "x";
        public const string Y           = "y";
        public const string X1          = "x1";
        public const string Y1          = "y1";
        public const string X2          = "x2";
        public const string Y2          = "y2";
        public const string R           = "r";
        public const string Width       = "width";
        public const string Height      = "height";
        public const string Transform   = "transform";

    }
}
