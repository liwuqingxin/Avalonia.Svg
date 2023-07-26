using System;
using Avalonia;
using Avalonia.Media;
using System.Linq;
using Nlnet.Avalonia.Svg.Utils;
// ReSharper disable MemberCanBePrivate.Global

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Base class for all svg renderable tags that implements the <see cref="ISvgRenderable"/>
/// </summary>
public abstract class SvgRenderable : SvgTagBase, ISvgRenderable
{
    protected SvgRenderable()
    {
        this.TryAddApplier(StyleApplier.Instance);
        this.TryAddApplier(DeferredPropertiesApplier.Instance);
    }

    public double? Opacity { get; set; }

    public Transform? Transform { get; set; }

    public string? ClipPath { get; set; }

    public string? Mask { get; set; }

    public virtual Rect Bounds { get; } = new Rect();

    /// <summary>
    /// Render the <see cref="ISvgRenderable"/>.
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="ctx"></param>
    public void Render(DrawingContext dc, ISvgContext ctx)
    { 
        using var stack = new StateStack();
        
        // ClipPath
        PushClipPath(dc, ctx, stack);

        // Mask
        PushMask(dc, ctx, stack);

        // Render
        RenderCore(dc, ctx);
    }

    private void PushClipPath(DrawingContext dc, ISvgContext ctx, StateStack stack)
    {
        if (this.ClipPath == null || !this.ClipPath.TryParseUrl(out var clipPathId, out _))
        {
            return;
        }
        if (!ctx.ClipPaths.TryGetValue(clipPathId, out var clipPath) || clipPath.Children == null)
        {
            return;
        }
        
        var geometryGroup = new GeometryGroup();
        foreach (var svgShape in clipPath.Children.OfType<ISvgShape>())
        {
            if (svgShape.OriginalGeometry == null)
            {
                continue;
            }
            var clone = svgShape.OriginalGeometry.Clone();
            if (svgShape.Transform != null)
            {
                clone.Transform = svgShape.Transform;
            }
            geometryGroup.Children.Add(clone);
        }

        if (geometryGroup.Children.Count <= 0)
        {
            return;
        }
        geometryGroup.Transform = clipPath.Transform;
        geometryGroup.FillRule  = FillRule.NonZero;
                    
        stack.Push(dc.PushGeometryClip(geometryGroup));
    }

    private void PushMask(DrawingContext dc, ISvgContext ctx, StateStack stack)
    {
        if (this.Mask == null || !this.Mask.TryParseUrl(out var maskId, out _))
        {
            return;
        }
        
        if (!ctx.Masks.TryGetValue(maskId, out var mask) || mask.Children != null)
        {
            return;
        }
        
        PushMaskElement(dc, ctx, stack, mask);
    }

    private bool PushMaskElement(DrawingContext dc, ISvgContext ctx, StateStack stack, ISvgRenderable maskElement)
    {
        if (maskElement.Children == null)
        {
            return false;
        }

        var rendered = false;

        using (dc.PushTransform(maskElement.Transform?.Value ?? Matrix.Identity))
        {
            using (dc.PushOpacity(maskElement.Opacity ?? 1))
            {
                foreach (var renderable in maskElement.Children.OfType<ISvgRenderable>())
                {
                    if (renderable is ISvgShape shape)
                    {
                        if (RenderWithMaskElement(dc, ctx, shape))
                        {
                            rendered = true;
                        }
                    }
                    else if (renderable is ISvgContainer c)
                    {
                        if (RenderWithMaskElementGroup(dc, ctx, c))
                        {
                            rendered = true;
                        }
                    }
                }
            }
        }

        return rendered;
    }

    private bool RenderWithMaskElement(DrawingContext dc, ISvgContext ctx, ISvgShape svgShape)
    {
        if (svgShape.OriginalGeometry == null)
        {
            return false;
        }

        var fill        = svgShape.GetPropertyValue<IFillSetter, IBrush>()?.Clone();
        var fillOpacity = svgShape.GetPropertyStructValue<IFillOpacitySetter, double>();
        if (fill == null)
        {
            return false;
        }
        if (fill is ISolidColorBrush solidColorBrush)
        {
            var rate = solidColorBrush.Color.ToHsv().V;
            fillOpacity *= rate;
        }
        if (svgShape is IOpacitySetter opacitySetter)
        {
            fillOpacity *= opacitySetter.Opacity ?? 1;
        }

        fill.Opacity = fillOpacity;
        using (dc.PushGeometryClip(svgShape.OriginalGeometry))
        {
            using (dc.PushOpacityMask(fill, svgShape.OriginalGeometry.Bounds))
            {
                RenderCore(dc, ctx);
            }
        }

        return true;
    }

    private bool RenderWithMaskElementGroup(DrawingContext dc, ISvgContext ctx, ISvgContainer svgContainer)
    {
        if (svgContainer.Children == null)
        {
            return false;
        }

        var rendered = false;

        foreach (var child in svgContainer.Children.OfType<ISvgShape>())
        {
            if (RenderWithMaskElement(dc, ctx, child))
            {
                rendered = true;
            }
        }

        return rendered;
    }

    /// <summary>
    /// Render the <see cref="ISvgRenderable"/>. In <see cref="SvgRenderable"/>, the <see cref="Render"/> pushes clip path if it exist.
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="ctx"></param>
    protected virtual void RenderCore(DrawingContext dc, ISvgContext ctx)
    {

    }
}
