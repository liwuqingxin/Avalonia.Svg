using Avalonia;
using Avalonia.Media;
using System.Collections.Generic;
// ReSharper disable MemberCanBePrivate.Global

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Base class for all svg renderable tags that implements the <see cref="ISvgRenderable"/>
/// </summary>
public abstract class SvgRenderable : SvgTagBase, ISvgRenderable, 
    IOpacitySetter, 
    ITransformSetter
{
    protected SvgRenderable()
    {
        this.TryAddApplier(StyleApplier.Instance);
        this.TryAddApplier(DeferredPropertiesApplier.Instance);
    }

    public double? Opacity { get; set; }

    public Transform? Transform { get; set; }

    public virtual Rect Bounds => Rect.Empty;

    public virtual Rect RenderBounds => Rect.Empty;

    /// <summary>
    /// Apply transforms. In <see cref="SvgRenderable"/>, it renders nothing.
    /// </summary>
    public virtual void ApplyTransforms(Stack<Matrix> transformsContext)
    {

    }

    /// <summary>
    /// Render the <see cref="ISvgRenderable"/>. In <see cref="SvgRenderable"/>, it renders nothing.
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="ctx"></param>
    public virtual void Render(DrawingContext dc, ISvgContext ctx)
    {
        
    }
}
