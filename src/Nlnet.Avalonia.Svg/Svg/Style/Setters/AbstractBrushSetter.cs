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
        Value = setterValue.ToIBrush();
    }

    public override void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        // url example : "url(#linearGradient-1)"
        var match = Regex.Match(deferredSetterValue, "url\\(\\#(.*)\\)");
        if (match.Success)
        {
            var id = match.Groups[1].Value;
            collector.Brushes.TryGetValue(id, out Value);
        }
    }
}
