using System;
using System.Text.RegularExpressions;
using Avalonia.Media;

namespace Avalonia.Svg.Setters;

/// <summary>
/// Abstract base setter factory.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AbstractSetterFactory<T> : ISvgStyleSetterFactory where T : ISvgStyleSetter, new()
{
    public ISvgStyleSetter CreateSetter()
    {
        return new T();
    }
}

/// <summary>
/// Abstract base setter for <see cref="IBrush"/>.
/// </summary>
public abstract class AbstractBrushSetter : ISvgStyleSetter
{
    protected IBrush? Brush;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Brush = Avalonia.Media.Brush.Parse(setterValue);
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        var match = Regex.Match(deferredSetterValue, "url\\(\\#(.*)\\)");
        if (match.Success)
        {
            var id = match.Groups[1].Value;
            collector.Brushes.TryGetValue(id, out Brush);
        }
    }
}

/// <summary>
/// Abstract base setter for <see cref="double"/>.
/// </summary>
public abstract class AbstractDoubleSetter : ISvgStyleSetter
{
    protected double? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = double.Parse(setterValue);
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        throw new NotImplementedException("Deferred double value is not implemented");
    }
}
