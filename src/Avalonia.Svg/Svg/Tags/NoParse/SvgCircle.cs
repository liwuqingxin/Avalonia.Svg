using System.Xml;

namespace Avalonia.Svg;

[SvgTag(SvgTags.circle)]
public class SvgCircleFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        return new SvgCircle();
    }
}

public class SvgCircle : SvgTagBase
{

}