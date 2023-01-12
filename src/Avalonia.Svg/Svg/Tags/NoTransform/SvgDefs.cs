using System.Xml;

namespace Avalonia.Svg;

[SvgTag(SvgTags.defs)]
public class SvgDefsFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        return new SvgDefs();
    }
}

public class SvgDefs : SvgTagBase
{
    
}