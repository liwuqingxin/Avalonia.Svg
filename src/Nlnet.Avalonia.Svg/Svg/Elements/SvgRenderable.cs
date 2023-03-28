using System;
using Avalonia;
using Avalonia.Media;
using System.Linq;
using Avalonia.Rendering;
using System.Threading.Tasks;
// ReSharper disable MemberCanBePrivate.Global

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Base class for all svg renderable tags that implements the <see cref="ISvgRenderable"/>
/// </summary>
public abstract class SvgRenderable : SvgTagBase, ISvgRenderable, 
    IOpacitySetter, 
    ITransformSetter,
    IClipPathSetter,
    IMaskSetter
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

    public virtual Rect Bounds => Rect.Empty;

    public virtual Rect RenderBounds => Rect.Empty;

    /// <summary>
    /// Render the <see cref="ISvgRenderable"/>.
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="ctx"></param>
    public void Render(DrawingContext dc, ISvgContext ctx)
    {
        DrawingContext.PushedState? clipPathPushedState = null;

        // ClipPath
        if (this.ClipPath != null && this.ClipPath.TryParseUrl(out var clipPathId, out _))
        {
            if (ctx.ClipPaths.TryGetValue(clipPathId, out var clipPath) && clipPath.Children != null)
            {
                var geometryGroup = new GeometryGroup();
                foreach (var svgShape in clipPath.Children.OfType<ISvgShape>())
                {
                    if (svgShape.RenderGeometry != null)
                    {
                        geometryGroup.Children.Add(svgShape.RenderGeometry);
                    }
                }
                geometryGroup.Transform = clipPath.Transform;
                geometryGroup.FillRule  = FillRule.NonZero;

                clipPathPushedState = dc.PushGeometryClip(geometryGroup);
            }
        }

        // Mask
        var rendered = false;
        if (this.Mask != null && this.Mask.TryParseUrl(out var maskId, out _))
        {
            if (ctx.Masks.TryGetValue(maskId, out var mask) && mask.Children != null)
            {
                rendered = RenderWithMaskElementGroup(dc, ctx, mask);
            }
        }
        
        if(rendered == false)
        {
            RenderCore(dc, ctx);
        }

        if (clipPathPushedState is IDisposable disposable3)
        {
            disposable3.Dispose();
        }
    }

    private bool RenderWithMaskElementGroup(DrawingContext dc, ISvgContext ctx, ISvgContainer container)
    {
        if (container.Children == null)
        {
            return false;
        }

        var rendered = false;

        using (dc.PushPostTransform(container.Transform?.Value ?? Matrix.Identity))
        {
            using (dc.PushOpacity(container.Opacity ?? 1))
            {
                foreach (var renderable in container.Children.OfType<ISvgRenderable>())
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
        if (svgShape.RenderGeometry == null)
        {
            return false;
        }

        var fill = svgShape.GetPropertyValue<IFillSetter, LightBrush>()?.Clone();
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
        using (dc.PushGeometryClip(svgShape.RenderGeometry))
        {
            using (dc.PushOpacityMask(fill, svgShape.RenderGeometry.Bounds))
            {
                RenderCore(dc, ctx);
            }
        }

        return true;
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
