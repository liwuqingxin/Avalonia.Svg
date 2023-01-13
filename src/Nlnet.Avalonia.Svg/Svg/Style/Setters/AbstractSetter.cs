using System;
using System.Text.RegularExpressions;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

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
    protected IBrush? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = global::Avalonia.Media.Brush.Parse(setterValue);
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

/// <summary>
/// Abstract base setter for <see cref="Geometry"/>.
/// </summary>
public abstract class AbstractGeometrySetter : ISvgStyleSetter
{
    protected Geometry? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = Geometry.Parse(setterValue);
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        throw new NotImplementedException("Deferred Geometry value is not implemented");
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

/// <summary>
/// Abstract base setter for <see cref="string"/>.
/// </summary>
public abstract class AbstractStringSetter : ISvgStyleSetter
{
    protected string? Value;

    public abstract void Set(ISvgTag tag);

    public void InitializeValue(string setterValue)
    {
        Value = setterValue;
    }

    public void InitializeDeferredValue(ISvgResourceCollector collector, string deferredSetterValue)
    {
        throw new NotImplementedException("Deferred string value is not implemented");
    }
}
