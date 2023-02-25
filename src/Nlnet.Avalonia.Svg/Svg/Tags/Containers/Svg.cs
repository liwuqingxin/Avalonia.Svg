using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.svg))]
public class Svg : SvgContainer, ISvg, ISvgContext, ISvgContainer, ISvgRenderable,
    IIdSetter,
    //IVersionSetter,
    //IStyleSetter,
    //IViewBoxSetter,
    IXSetter,
    IYSetter
{
    private Transform? _alignTransform;

    public static Svg Empty { get; } = new();

    public string? Id
    {
        get;
        set;
    }

    public double? X
    {
        get;
        set;
    }

    public double? Y
    {
        get;
        set;
    }

    private Dictionary<string, ISvgStyle> Styles { get; } = new();

    private Dictionary<string, LightBrush> Brushes { get; } = new();

    private Dictionary<string, ISvgTag> IdTags { get; } = new();

    private List<ISvgRenderable> Renderables { get; } = new();



    #region ISvgContext

    IReadOnlyDictionary<string, ISvgStyle> ISvgContext.Styles => this.Styles;

    IReadOnlyDictionary<string, LightBrush> ISvgContext.Brushes => this.Brushes;

    IReadOnlyDictionary<string, ISvgTag> ISvgContext.IdTags => this.IdTags;

    IReadOnlyList<ISvgRenderable> ISvgContext.Renderables => this.Renderables;

    #endregion



    #region ISvg

    void ISvg.Render(DrawingContext dc)
    {
        using (dc.PushSetTransform(_alignTransform?.Value ?? Matrix.Identity))
        {
            this.Children?.RenderRecursively(dc);
        }
    }

    Size ISvg.GetRenderSize()
    {
        var left   = 0d;
        var top    = 0d;
        var right  = 0d;
        var bottom = 0d;

        foreach (var renderable in Renderables)
        {
            left   = Math.Min(renderable.RenderBounds.Left, left);
            top    = Math.Min(renderable.RenderBounds.Top,  top);
            right  = Math.Max(renderable.RenderBounds.Right,  right);
            bottom = Math.Max(renderable.RenderBounds.Bottom, bottom);
        }

        return new Size(right - left, bottom - top);
    }

    #endregion



    internal void PrepareContext()
    {
        // Prepare all elements with id.
        this.VisitSvgTagTree(tag =>
        {
            if (string.IsNullOrEmpty(tag.Id) == false)
            {
                // BUG If id is duplicate, now we drop it.
                this.IdTags.TryAdd(tag.Id, tag);
            }
        });

        // Prepare styles, brushes, renderables...
        this.VisitSvgTagTree(tag =>
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (tag is ISvgStyleProvider styleProvider)
            {
                foreach (var style in styleProvider.GetStyles())
                {
                    this.Styles.Add(style.Class, style);
                }
            }

            if (tag is ISvgBrushProvider brushProvider)
            {
                if (brushProvider.Id != null)
                {
                    this.Brushes.Add(brushProvider.Id, brushProvider.GetBrush(this));
                }
            }

            if (tag is ISvgRenderable renderable)
            {
                this.Renderables.Add(renderable);
            }
        });
    }

    internal void BuildContext()
    {
        this.VisitSvgTagTree(tag =>
        {
            if (tag is ISvgStyleProvider provider)
            {
                var styles = provider.GetStyles();
                foreach (var style in styles)
                {
                    foreach (var setter in style.Setters)
                    {
                        setter.ApplyDeferredValueString(this);
                    }
                }
            }
        });
    }

    public override void ApplyContext(ISvgContext context)
    {
        if (Children == null)
        {
            return;
        }

        foreach (var child in Children)
        {
            child.VisitSvgTagTree(tag =>
            {
                tag.ApplyContext(context);
            });
        }

        _alignTransform = SvgHelper.GetAlignToTopLeftTransform(Renderables.Select(v => v.Bounds));
    }
}