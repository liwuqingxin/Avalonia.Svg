using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.svg))]
public class Svg : SvgRenderable, ISvg, ISvgResourceCollector, ISvgContainer, ISvgRenderable,
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

    private Dictionary<string, ISvgClassStyle> Styles { get; } = new();

    private Dictionary<string, IBrush> Brushes { get; } = new();

    private List<ISvgRenderable> Renderables { get; } = new();



    #region ISvgResourceCollector

    IReadOnlyDictionary<string, ISvgClassStyle> ISvgResourceCollector.Styles => this.Styles;

    IReadOnlyDictionary<string, IBrush> ISvgResourceCollector.Brushes => this.Brushes;

    IReadOnlyList<ISvgRenderable> ISvgResourceCollector.Renderables => this.Renderables;

    void ISvgResourceCollector.CollectResources()
    {
        this.VisitSvgTagTree(tag =>
        {
            switch (tag)
            {
                case ISvgStyleProvider styleProvider:
                    {
                        foreach (var style in styleProvider.GetStyles())
                        {
                            this.Styles.Add(style.Class, style);
                        }
                        break;
                    }
                case ISvgBrushProvider brushProvider:
                    if (brushProvider.Id != null)
                    {
                        this.Brushes.Add(brushProvider.Id, brushProvider.GetBrush());
                    }
                    break;
                case ISvgRenderable renderable:
                    this.Renderables.Add(renderable);
                    break;
            }
        });
    }

    #endregion



    public override void ApplyResources(ISvgResourceCollector collector)
    {
        if (Children == null)
        {
            return;
        }

        foreach (var child in Children)
        {
            child.VisitSvgTagTree(tag =>
            {
                tag.ApplyResources(collector);
            });
        }

        _alignTransform = SvgHelper.GetAlignToTopLeftTransform(Renderables.Select(v => v.Bounds));
    }

    void ISvg.Render(DrawingContext dc)
    {
        using (dc.PushTransformContainer())
        {
            using (dc.PushSetTransform(_alignTransform?.Value ?? Matrix.Identity))
            {
                this.Children?.Render(dc);
            }
        }
    }

    Size ISvg.GetRenderSize()
    {
        var left = 0d;
        var top = 0d;
        var right = 0d;
        var bottom = 0d;

        foreach (var renderable in Renderables)
        {
            left = Math.Min(renderable.RenderBounds.Left, left);
            top = Math.Min(renderable.RenderBounds.Top, top);
            right = Math.Max(renderable.RenderBounds.Right, right);
            bottom = Math.Max(renderable.RenderBounds.Bottom, bottom);
        }

        return new Size(right - left, bottom - top);
    }
}