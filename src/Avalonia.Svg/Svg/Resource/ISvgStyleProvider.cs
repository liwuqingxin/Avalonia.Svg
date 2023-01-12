using System.Collections.Generic;

namespace Avalonia.Svg;

/// <summary>
/// Svg style provider.
/// </summary>
public interface ISvgStyleProvider
{
    /// <summary>
    /// Provide a list of svg style.
    /// </summary>
    /// <returns></returns>
    IEnumerable<ISvgClassStyle> GetStyles();
}
