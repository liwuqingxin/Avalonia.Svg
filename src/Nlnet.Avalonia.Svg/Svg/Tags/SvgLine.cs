using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;
using System.Collections.Generic;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.line))]
public class SvgLine : SvgTagBase, 
    IClassSetter,
    IX1Setter,
    IX2Setter,
    IY1Setter,
    IY2Setter,
    ISvgVisual
{
    private Rect? _bounds;
    private Rect _renderBounds;

    public string? Class
    {
        get;
        set;
    }
    public double? X1
    {
        get;
        set;
    }
    public double? X2
    {
        get;
        set;
    }
    public double? Y1
    {
        get;
        set;
    }
    public double? Y2
    {
        get;
        set;
    }
    public IBrush? Fill
    {
        get;
        set;
    }
    public IBrush? Stroke
    {
        get;
        set;
    }
    public double? StrokeWidth
    {
        get;
        set;
    }
    public double? Opacity
    {
        get;
        set;
    }

    public SvgLine()
    {
        ResourceAppliers = new List<ISvgResourceApplier>()
        {
            new ClassApplier(),
            new DeferredPropertiesApplier(),
        };
    }

    Rect ISvgVisual.Bounds
    {
        get
        {
            if (_bounds != null)
            {
                return _bounds.Value;
            }
            if (X1 == null || X2 == null || Y1 == null || Y2 == null)
            {
                return Rect.Empty;
            }
            _bounds = new Rect(new Point(X1.Value, Y1.Value), new Point(X2.Value, Y2.Value));

            return _bounds.Value;
        }
    }

    Rect ISvgVisual.RenderBounds => _renderBounds;

    void ISvgVisual.Render(DrawingContext dc)
    {
        dc.RenderWithOpacity(Opacity, () =>
        {
            var x1 = _renderBounds.Left;
            var y1 = _renderBounds.Top;
            var x2 = _renderBounds.Right;
            var y2 = _renderBounds.Bottom;
            dc.DrawLine(new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), new Point(x1, y1), new Point(x2, y2));
        });
    }

    void ISvgVisual.ApplyTransform(Transform transform)
    {
        _renderBounds = ((ISvgVisual)this).Bounds.TransformToAABB(transform.Value);
    }
}