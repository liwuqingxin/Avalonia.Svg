using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;
using SkiaSharp;
using System;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.path))]
public class SvgPath : Markerable, ISvgShape, ISvgGraphic, ISvgRenderable, IMarkerable,
    IDataSetter
{
    public Geometry? Data { get; set; }

    protected override Geometry? OnCreateOriginalGeometry()
    {
        return Data;
    }

    public override double GetMarkerOrientDegree(SKPath path, int index)
    {
        return 90;
    }
}
