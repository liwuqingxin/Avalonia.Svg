﻿using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.line))]
public class SvgLine : SvgShape, ISvgShape, ISvgGraphic, ISvgRenderable,
    IX1Setter,
    IX2Setter,
    IY1Setter,
    IY2Setter
{
    public string? Class { get; set; }
    public double? X1    { get; set; }
    public double? X2    { get; set; }
    public double? Y1    { get; set; }
    public double? Y2    { get; set; }

    public SvgLine()
    {
        this.TryAddApplier(new ClassApplier());
        this.TryAddApplier(new DeferredPropertiesApplier());
    }

    public override void OnPropertiesFetched()
    {
        if (X1 != null && X2 != null && Y1 != null && Y2 != null)
        {
            OriginalGeometry = new LineGeometry(new Point(X1.Value, Y1.Value), new Point(X2.Value, Y2.Value));
        }
    }
}