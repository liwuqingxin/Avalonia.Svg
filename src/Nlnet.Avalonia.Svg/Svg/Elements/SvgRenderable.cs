using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Utilities;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Mixins;
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

        if (this.Mask != null && this.Mask.TryParseUrl(out var maskId, out _))
        {
            if (ctx.Masks.TryGetValue(maskId, out var mask) && mask.Children != null)
            {
                foreach (var svgShape in mask.Children.OfType<ISvgShape>())
                {
                    if (svgShape.RenderGeometry == null)
                    {
                        continue;
                    }

                    var fill        = svgShape.GetPropertyValue<IFillSetter, LightBrush>()?.Clone();
                    var fillOpacity = svgShape.GetPropertyStructValue<IFillOpacitySetter, double>();

                    using (dc.PushGeometryClip(svgShape.RenderGeometry))
                    {
                        1
                        // TODO fill-opacity and no mask found situation.
                        using(dc.PushOpacityMask(fill, svgShape.RenderGeometry.Bounds))
                        {
                            RenderCore(dc, ctx);
                        }
                    }
                }
            }
        }
        else
        {
            RenderCore(dc, ctx);
        }

        if (clipPathPushedState is IDisposable disposable3)
        {
            disposable3.Dispose();
        }
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
