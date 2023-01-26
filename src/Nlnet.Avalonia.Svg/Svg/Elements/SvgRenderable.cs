using Avalonia;
using Avalonia.Media;
// ReSharper disable MemberCanBePrivate.Global

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Base class for all svg renderable tags that implements the <see cref="ISvgRenderable"/>
/// </summary>
public abstract class SvgRenderable : SvgTagBase, ISvgRenderable, 
    IOpacitySetter, 
    ITransformSetter, 
    IFillSetter, 
    IStrokeSetter, 
    IStrokeWidthSetter
{
    private TransformGroup? _transformGroup;

    /// <summary>
    /// The original geometry that the svg describes.
    /// </summary>
    protected Geometry? OriginalGeometry;

    /// <summary>
    /// The geometry to render, which applied some transforms.
    /// </summary>
    protected Geometry? RenderGeometry;

    public double?             Opacity     { get; set; }
    public Transform?          Transform   { get; set; }
    IBrush? IFillSetter.       Fill        { get; set; }
    IBrush? IStrokeSetter.     Stroke      { get; set; }
    double? IStrokeWidthSetter.StrokeWidth { get; set; }

    Rect ISvgRenderable.Bounds => OriginalGeometry?.Bounds ?? Rect.Empty;

    Rect ISvgRenderable.RenderBounds => RenderGeometry?.Bounds ?? Rect.Empty;

    void ISvgRenderable.ApplyGlobalTransform(Transform transform)
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

    /// <summary>
    /// Render the <see cref="ISvgRenderable"/>. In <see cref="SvgRenderable"/>, it renders the <see cref="RenderGeometry"/>.
    /// </summary>
    /// <param name="dc"></param>
    public virtual void Render(DrawingContext dc)
    {
        if (RenderGeometry == null)
        {
            return;
        }

        var fill = this.GetPropertyValue<IFillSetter, IBrush>() ?? Brushes.Black;
        var stroke = this.GetPropertyValue<IStrokeSetter, IBrush>() ?? Brushes.Black;
        var strokeWidth = this.GetPropertyStructValue<IStrokeWidthSetter, double>() ?? 0d;
        void DoRender() => dc.DrawGeometry(fill, new Pen(stroke, strokeWidth), RenderGeometry);

        if (Opacity != null)
        {
            using (dc.PushOpacity(Opacity.Value))
            {
                DoRender();
            }
        }
        else
        {
            DoRender();
        }
    }
}
