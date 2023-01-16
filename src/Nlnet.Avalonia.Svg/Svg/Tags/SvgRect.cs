using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.rect))]
public class SvgRect : SvgTagBase, 
    ISvgVisual, 
    IXSetter, 
    IYSetter, 
    IIdSetter, 
    IWidthSetter, 
    IHeightSetter
{
    public double? X           { get; set; }
    public double? Y           { get; set; }
    public string? Id          { get; set; }
    public double? Width       { get; set; }
    public double? Height      { get; set; }
    public IBrush? Fill        { get; set; }
    public IBrush? Stroke      { get; set; }
    public double? StrokeWidth { get; set; }
    public double? Opacity     { get; set; }



    #region ISvgVisual

    public Rect Bounds => new Rect(X ?? 0, Y ?? 0, Width ?? 0, Height ?? 0);

    public Rect RenderBounds { get; private set; }
    

    void ISvgVisual.Render(DrawingContext dc)
    {
        if (Width == null || Height == null || (Width == 0 && Height == 0))
        {
            return;
        }

        dc.RenderWithOpacity(Opacity, () =>
        {
            dc.DrawRectangle(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), RenderBounds);
        });
    }

    void ISvgVisual.ApplyTransform(Transform transform)
    {
        RenderBounds = Bounds.TransformToAABB(transform.Value);
    }

    #endregion
}