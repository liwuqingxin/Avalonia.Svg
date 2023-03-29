﻿using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;
using SkiaSharp;
using System;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : SvgMarkerable, ISvgShape, ISvgGraphic, ISvgRenderable, ISvgMarkerable,
    IDataSetter
{
    public Geometry? Data { get; set; }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        return Data;
    }

    protected override double GetMarkerOrientRadians(SKPath path, int index)
    {
        return 90;
    }
}
