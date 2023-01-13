using System.Xml;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

// TODO As Mask resource.

[SvgTag(SvgTags.mask)]
public class SvgMaskFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new SvgMask();
        xmlNode.Attributes?.FetchPropertiesTo(tag);
        return tag;
    }
}

public class SvgMask : SvgTagBase, 
    IIdSetter, IFillSetter
{
    public string? Id   { get; set; }
    public IBrush? Fill { get; set; }
}