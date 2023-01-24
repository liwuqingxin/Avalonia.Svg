using System.Xml;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.defs)]
public class SvgDefsFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        return new SvgDefs();
    }
}

public class SvgDefs : SvgTagBase, ISvgContainer
{
    
}