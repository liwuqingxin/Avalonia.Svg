using System.Xml;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.g)]
public class SvgGroupFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        return new SvgGroup();
    }
}

public class SvgGroup : SvgTagBase
{

}