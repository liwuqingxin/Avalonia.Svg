using System.Text.RegularExpressions;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base setter for <see cref="IBrush"/>.
/// </summary>
public abstract class AbstractBrushSetter : ISvgStyleSetter
{
    protected IBrush? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = setterValue.ToIBrush();
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
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
