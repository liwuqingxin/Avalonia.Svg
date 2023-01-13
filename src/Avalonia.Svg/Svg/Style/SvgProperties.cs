using Avalonia.Media;

namespace Avalonia.Svg
{
    /// <summary>
    /// All svg property types.
    /// </summary>
    public static class SvgTypes
    {
        public const string Double    = "double";
        public const string Bool      = "bool";
        public const string Int       = "int";
        public const string Brush     = nameof(IBrush);
        public const string Thickness = nameof(Avalonia.Thickness);
    }

    /// <summary>
    /// All svg property definitions.
    /// </summary>
    public static class SvgProperties
    {
        public const string Data        = "d";
        public const string Class       = "class";
        public const string Fill        = "fill";
        public const string Stroke      = "stroke";
        public const string StrokeWidth = "stroke-width";
        public const string Opacity     = "opacity";
    }
}
