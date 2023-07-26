using System.Text.RegularExpressions;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="IBrush"/>.
/// </summary>
public abstract class AbstractBrushSetter : AbstractDeferredSetter
{
    protected IBrush? Value;

    public override void InitializeValue(string setterValue)
    {
        Value = setterValue.ToBrush();
    }

    public override void InitializeDeferredValue(ISvgContext context, string deferredSetterValue)
    {
        //
        // https://www.w3.org/TR/SVG2/painting.html#SpecifyingPaint
        // url example : "url(#linearGradient-1)" or "url(#linearGradient-1) blue"
        //
        deferredSetterValue.TryParseUrl(out var id, out var defaultToken);
        context.Brushes.TryGetValue(id, out Value);
        if (Value == null && string.IsNullOrEmpty(defaultToken) == false)
        {
            defaultToken.TryToBrush(out Value);
        }
    }
}
