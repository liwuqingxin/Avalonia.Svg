using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Base class for all svg visual tags that implements the <see cref="ISvgVisual"/>
/// </summary>
public abstract class SvgVisualBase : SvgTagBase, ISvgVisual
{
    private TransformGroup? _transformGroup;

    protected Geometry? OriginalGeometry;
    protected Geometry? RenderGeometry;

    public double?    Opacity     { get; set; }
    public Transform? Transform   { get; set; }
    public IBrush?    Fill        { get; set; }
    public IBrush?    Stroke      { get; set; }
    public double?    StrokeWidth { get; set; }

    public Rect Bounds => OriginalGeometry?.Bounds ?? Rect.Empty;

    public Rect RenderBounds => RenderGeometry?.Bounds ?? Rect.Empty;

    public void ApplyTransform(Transform transform)
    {
        _transformGroup ??= new TransformGroup();
        _transformGroup.Children.Add(transform);
        if (Transform != null)
        {
            _transformGroup.Children.Add(Transform);
        }

        ApplyTransformCore(_transformGroup);
    }

    /// <summary>
    /// Derived class does not need to care about any transform from outer except the parameter <see cref="transform"/>.
    /// </summary>
    /// <param name="transform"></param>
    protected virtual void ApplyTransformCore(Transform transform)
    {
        if (OriginalGeometry == null)
        {
            return;
        }

        RenderGeometry = OriginalGeometry.Clone();
        RenderGeometry.Transform = transform;
    }

    public virtual void ApplyAncestorTransform(Transform transform)
    {
        _transformGroup ??= new TransformGroup();
        _transformGroup.Children.Add(transform);
    }

    public virtual void Render(DrawingContext dc)
    {
        if (RenderGeometry == null)
        {
            return;
        }

        dc.RenderWithOpacity(Opacity, () =>
        {
            dc.DrawGeometry(Fill ?? Brushes.Black, new Pen(Stroke ?? Brushes.Black, StrokeWidth ?? 0), RenderGeometry);
        });
    }
}
