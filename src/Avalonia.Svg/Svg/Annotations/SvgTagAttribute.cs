using System;

namespace Avalonia.Svg;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class SvgTagAttribute : Attribute
{
    public SvgTags Tag { get; set; }

    public SvgTagAttribute(SvgTags tag)
    {
        Tag = tag;
    }
}
