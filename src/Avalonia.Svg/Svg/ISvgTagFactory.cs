using System.Xml;

namespace Avalonia.Svg;

/// <summary>
/// Svg tag factory definition.
/// </summary>
public interface ISvgTagFactory
{
    /// <summary>
    /// Create a new tag from <see cref="XmlNode"/> from svg document.
    /// </summary>
    /// <param name="xmlNode"></param>
    /// <returns></returns>
    ISvgTag CreateTag(XmlNode xmlNode);
}
