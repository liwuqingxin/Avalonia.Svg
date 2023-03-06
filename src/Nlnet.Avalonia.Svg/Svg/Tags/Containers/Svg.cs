using System;
using System.Collections.Generic;
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

    private readonly Dictionary<string, ISvgStyle>  _styles      = new();
    private readonly Dictionary<string, LightBrush> _brushes     = new();
    private readonly Dictionary<string, ISvgTag>    _idTags      = new();
    private readonly List<ISvgRenderable>           _renderables = new();



    #region ISvgContext

    IReadOnlyDictionary<string, ISvgStyle> ISvgContext.Styles => this._styles;

    IReadOnlyDictionary<string, LightBrush> ISvgContext.Brushes => this._brushes;

    IReadOnlyDictionary<string, ISvgTag> ISvgContext.IdTags => this._idTags;

    IReadOnlyList<ISvgRenderable> ISvgContext.Renderables => this._renderables;

    #endregion



    #region ISvg

    void ISvg.Render(DrawingContext dc)
    {
        this.Children?.RenderRecursively(dc);
    }

    Size ISvg.GetRenderSize()
    {
        var left   = 0d;
        var top    = 0d;
        var right  = 0d;
        var bottom = 0d;

        foreach (var renderable in _renderables)
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
                this._idTags.TryAdd(tag.Id, tag);
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
                    this._styles.Add(style.Class, style);
                }
            }

            if (tag is ISvgBrushProvider brushProvider)
            {
                if (brushProvider.Id != null)
                {
                    this._brushes.Add(brushProvider.Id, brushProvider.GetBrush(this));
                }
            }

            if (tag is ISvgRenderable renderable)
            {
                this._renderables.Add(renderable);
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
    }
}