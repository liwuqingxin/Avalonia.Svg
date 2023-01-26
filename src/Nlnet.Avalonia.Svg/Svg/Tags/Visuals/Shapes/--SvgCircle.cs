using System.Xml;

namespace Nlnet.Avalonia.Svg;

[SvgTag(SvgTags.circle)]
public class SvgCircleFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        return new SvgCircle();
    }
}

public class SvgCircle : SvgRenderable, ISvgShape, ISvgGraphic, ISvgRenderable
{

}