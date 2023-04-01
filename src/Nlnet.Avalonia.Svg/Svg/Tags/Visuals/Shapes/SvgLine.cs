using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.line))]
public class SvgLine : SvgMarkerable, ISvgShape, ISvgGraphic, ISvgRenderable, ISvgMarkerable,
    IX1Setter,
    IX2Setter,
    IY1Setter,
    IY2Setter
{
    public double? X1 { get; set; }
    public double? X2 { get; set; }
    public double? Y1 { get; set; }
    public double? Y2 { get; set; }


    protected override Geometry? OnCreateOriginalGeometry()
    {
        if (X1 == null || Y1 == null || X2 == null || Y2 == null)
        {
            return null;
        }

        return new LineGeometry(new Point(X1.Value, Y1.Value), new Point(X2.Value, Y2.Value));
    }

    public override void RenderMarkerStart(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        if (OriginalGeometry is LineGeometry line)
        {
            var point  = line.StartPoint;
            var radian = CalculateRadian(line.StartPoint, line.EndPoint);
            RenderMarkerOnPoint(dc, ctx, marker, point, radian, true);
        }
    }

    public override void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        if (OriginalGeometry is LineGeometry line)
        {
            var point   = line.EndPoint;
            var radian = CalculateRadian(line.StartPoint, line.EndPoint);
            RenderMarkerOnPoint(dc, ctx, marker, point, radian, false);
        }
    }

    public override void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker)
    {
        // Noop
    }
}