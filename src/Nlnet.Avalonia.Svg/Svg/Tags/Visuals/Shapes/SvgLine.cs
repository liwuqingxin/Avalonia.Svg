using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Nlnet.Avalonia.Svg.CompileGenerator;
using SkiaSharp;
using System;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.line))]
public class SvgLine : SvgMarkerable, ISvgShape, ISvgGraphic, ISvgRenderable, ISvgMarkerable,
    IX1Setter,
    IX2Setter,
    IY1Setter,
    IY2Setter
{
    public double? X1    { get; set; }
    public double? X2    { get; set; }
    public double? Y1    { get; set; }
    public double? Y2    { get; set; }
    

    protected override Geometry? OnCreateOriginalGeometry()
    {
        if (X1 == null || Y1 == null || X2 == null || Y2 == null)
        {
            return null;
        }

        return new LineGeometry(new Point(X1.Value, Y1.Value), new Point(X2.Value, Y2.Value));
    }

    protected override double GetMarkerOrientRadians(SKPath path, int index)
    {
        return 90;
    }
}