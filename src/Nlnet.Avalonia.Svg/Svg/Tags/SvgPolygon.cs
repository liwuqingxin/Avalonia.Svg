using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.polygon))]
public class SvgPolygon : SvgVisualBase,
    IClassSetter,
    IPointsSetter,
    ISvgVisual
{
    private Geometry? _geometry;
    private Geometry? _renderGeometry;

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

    public override Rect Bounds
    {
        get
        {
            if (Points == null)
            {
                return Rect.Empty;
            }

            if (_geometry != null)
            {
                return _geometry.Bounds;
            }

            _geometry = new PolylineGeometry(Points, true);
            return _geometry.Bounds;
        }
    }

    public override Rect RenderBounds => _renderGeometry?.Bounds ?? Rect.Empty;

    public override void Render(DrawingContext dc)
    {
        if (_renderGeometry == null)
        {
            return;
        }

        dc.RenderWithOpacity(Opacity, () =>
        {
            dc.DrawGeometry(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), _renderGeometry);
        });
    }

    protected override void ApplyTransformCore(Transform transform)
    {
        if (_geometry == null)
        {
            return;
        }

        _renderGeometry = _geometry.Clone();
        _renderGeometry.Transform = transform;
    }
}