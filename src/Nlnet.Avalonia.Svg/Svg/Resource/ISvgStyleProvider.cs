using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg style provider.
/// </summary>
public interface ISvgStyleProvider
{
    /// <summary>
    /// Provide a list of svg style.
    /// </summary>
    /// <returns></returns>
    IEnumerable<ISvgStyle> GetStyles();
}
