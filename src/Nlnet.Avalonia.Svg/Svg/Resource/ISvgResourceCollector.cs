using System.Collections.Generic;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Collector to collect svg resources.
/// </summary>
public interface ISvgResourceCollector
{
    /// <summary>
    /// All styles.
    /// </summary>
    public IReadOnlyDictionary<string, ISvgClassStyle> Styles { get; }

    /// <summary>
    /// All brushes.
    /// </summary>
    public IReadOnlyDictionary<string, IBrush> Brushes { get; }

    /// <summary>
    /// All visuals.
    /// </summary>
    public IReadOnlyList<ISvgVisual> Visuals { get; }

    /// <summary>
    /// Collect resources.
    /// </summary>
    public void CollectResources();
}