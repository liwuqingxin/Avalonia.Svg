﻿using System.Xml;
using Avalonia.Media;

namespace Avalonia.Svg;

[SvgTag(SvgTags.stop)]
public class SvgStopFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        if (xmlNode.Attributes != null)
        {
            var offset = xmlNode.Attributes["offset"]?.Value.ToDouble() ?? 0;
            var stopOpacity = xmlNode.Attributes["stop-opacity"]?.Value.ToDouble() ?? 1;
            var stopColor = Color.Parse(xmlNode.Attributes["stop-color"]?.Value ?? "Black");
            return new SvgStop(offset, stopOpacity, stopColor);
        }
        else
        {
            return new SvgStop(0, 1, Colors.Black);
        }
    }
}

public class SvgStop : SvgTagBase
{
    public GradientStop GradientStop { get; set; }

    public SvgStop(double offset, double opacity, Color color)
    {
        GradientStop = new GradientStop
        {
            //
            // NOTE Opacity of GradientStop is not supported in Avalonia 11.0.0-preview4.
            // So we apply it to the color.
            //
            Color = Color.FromArgb((byte)(opacity * color.A), color.R, color.G, color.B),
            Offset = offset
        };
    }
}