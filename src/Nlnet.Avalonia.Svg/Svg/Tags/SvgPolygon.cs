﻿using System.Collections.Generic;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polygon))]
public class SvgPolygon : SvgVisualBase,
    IClassSetter,
    IPointsSetter
{
    public string? Class { get; set; }

    public PointList? Points { get; set; }

    public SvgPolygon()
    {
        ResourceAppliers = new List<ISvgResourceApplier>()
        {
            new ClassApplier(),
            new DeferredPropertiesApplier(),
        };
    }

    public override void OnPropertiesFetched()
    {
        OriginalGeometry = Points == null ? new PolylineGeometry() : new PolylineGeometry(Points, true);
    }
}