﻿using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg brush provider.
/// </summary>
public interface ISvgBrushProvider
{
    /// <summary>
    /// Id of the brush.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Brush instance.
    /// </summary>
    /// <returns></returns>
    public LightBrush GetBrush(ISvgContext context);
}
