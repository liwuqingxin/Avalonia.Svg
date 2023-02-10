using System.Text.RegularExpressions;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="LightBrush"/>.
/// </summary>
public abstract class AbstractBrushSetter : AbstractDeferredSetter
{
    protected LightBrush? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToLightBrush();
    }

    public override void InitializeDeferredValue(ISvgContext context, string deferredSetterValue)
    {
        // https://www.w3.org/TR/SVG2/painting.html#SpecifyingPaint
        // url example : "url(#linearGradient-1)" or "url(#linearGradient-1) blue"
        var match = Regex.Match(deferredSetterValue, "url\\(\\#(.*)\\)\\s*(.*)");
        if (match.Success)
        {
            var id = match.Groups[1].Value;
            context.Brushes.TryGetValue(id, out Value);
            if (Value == null && match.Groups.Count > 2)
            {
                var defaultValueString = match.Groups[2].Value;
                defaultValueString.TryToLightBrush(out Value);
            }
        }
    }
}
