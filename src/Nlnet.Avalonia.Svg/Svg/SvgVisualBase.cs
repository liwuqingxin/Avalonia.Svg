using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg;

public abstract class SvgVisualBase : SvgTagBase, ISvgVisual
{
    private TransformGroup? _transformGroup;

    public double?    Opacity     { get; set; }
    public Transform? Transform   { get; set; }
    public IBrush?    Fill        { get; set; }
    public IBrush?    Stroke      { get; set; }
    public double?    StrokeWidth { get; set; }

    public abstract Rect Bounds { get; }

    public abstract Rect RenderBounds { get; }

    public abstract void Render(DrawingContext dc);

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
    protected abstract void ApplyTransformCore(Transform transform);

    public virtual void ApplyAncestorTransform(Transform transform)
    {
        _transformGroup ??= new TransformGroup();
        _transformGroup.Children.Add(transform);
    }
}
