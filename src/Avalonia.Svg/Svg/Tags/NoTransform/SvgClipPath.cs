using System.Xml;

namespace Avalonia.Svg;

[SvgTag(SvgTags.clipPath)]
public class SvgClipPathFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgClipPath();

        if (xmlNode.Attributes != null)
        {
            tag.Id = xmlNode.Attributes["id"]?.Value;
        }

        return tag;
    }
}

public class SvgClipPath : SvgTagBase
{
    public string? Id { get; set; }
}