using Avalonia;
using Avalonia.Media;
using Avalonia.Utilities;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Base class for all svg renderable tags that implements the <see cref="ISvgRenderable"/>
/// </summary>
public abstract class SvgRenderable : SvgTagBase, ISvgRenderable, 
    IOpacitySetter, 
    ITransformSetter,
    IClipPathSetter
{
    protected SvgRenderable()
    {
        this.TryAddApplier(StyleApplier.Instance);
        this.TryAddApplier(DeferredPropertiesApplier.Instance);
    }

    public double? Opacity { get; set; }

    public Transform? Transform { get; set; }

    public string? ClipPath { get; set; }

    public virtual Rect Bounds => Rect.Empty;

    public virtual Rect RenderBounds => Rect.Empty;

    /// <summary>
    /// Render the <see cref="ISvgRenderable"/>.
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="ctx"></param>
    public void Render(DrawingContext dc, ISvgContext ctx)
    {
        var useClipPath = false;

        if (this.ClipPath != null && this.ClipPath.TryParseUrl(out var id, out var token))
        {
            if (ctx.ClipPaths.TryGetValue(id, out var clipPath) && clipPath.Children != null)
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

                using (dc.PushGeometryClip(geometryGroup))
                {
                    RenderCore(dc, ctx);
                }
                useClipPath = true;
            }
        }

        if (!useClipPath)
        {
            RenderCore(dc, ctx);
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
