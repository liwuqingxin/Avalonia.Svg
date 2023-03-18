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
    IViewBoxSetter,
    IPreserveAspectRatioSetter,
    IXSetter,
    IYSetter,
    IWidthSetter,
    IHeightSetter
{
    public static Svg Empty { get; } = new();

    public string? Id
    {
        get;
        set;
    }

    public ViewBox? ViewBox
    {
        get;
        set;
    }

    public PreserveAspectRatio? PreserveAspectRatio
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

    public double? Width
    {
        get;
        set;
    }

    public double? Height
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

    void ISvg.Render(DrawingContext dc, Size availableSize)
    {
        var viewBox = ViewBox ?? new ViewBox(0, 0, availableSize.Width, availableSize.Height);
        var viewBoxSize = new Size(viewBox.Width, viewBox.Height);

        GetUniformFactors(availableSize, viewBoxSize, false, out var scale1, out var offsetX1, out var offsetY1);

        // The view box should always stretch to the available region and stay on center.
        using (dc.PushSetTransform(Matrix.CreateScale(scale1, scale1)))
        using (dc.PushPostTransform(Matrix.CreateTranslation(offsetX1, offsetY1)))
        using (dc.PushTransformContainer())
        {
            var ratio = PreserveAspectRatio ?? new PreserveAspectRatio(PreserveAspectRatioAlign.xMidYMid, PreserveAspectRatioMeetOrSlice.meet);
            if (ratio.Align == PreserveAspectRatioAlign.none)
            {
                GetFillFactors(viewBoxSize, RenderBounds.Size, out var scaleX2, out var scaleY2);
                // If the BorderThickness should be taken into account, uncomment it.
                //using (dc.PushPostTransform(Matrix.CreateTranslation(-RenderBounds.Left, -RenderBounds.Top)))
                using (dc.PushPostTransform(Matrix.CreateScale(scaleX2, scaleY2)))
                using (dc.PushTransformContainer())
                    this.Children?.RenderRecursively(dc);
            }
            else
            {
                var isSlice = ratio.MeetOrSlice == PreserveAspectRatioMeetOrSlice.slice;
                GetUniformFactors(viewBoxSize, new Size(RenderBounds.Right, RenderBounds.Bottom), isSlice, out var scale2, out var offsetX2, out var offsetY2);
                switch (ratio.Align)
                {
                    case PreserveAspectRatioAlign.xMinYMin:
                        offsetX2 = 0;
                        offsetY2 = 0;
                        break;
                    case PreserveAspectRatioAlign.xMidYMin:
                        offsetY2 = 0;
                        break;
                    case PreserveAspectRatioAlign.xMaxYMin:
                        offsetX2 *= 2;
                        offsetY2 =  0;
                        break;
                    case PreserveAspectRatioAlign.xMinYMid:
                        offsetX2 = 0;
                        break;
                    case PreserveAspectRatioAlign.xMidYMid:
                        break;
                    case PreserveAspectRatioAlign.xMaxYMid:
                        offsetX2 *= 2;
                        break;
                    case PreserveAspectRatioAlign.xMinYMax:
                        offsetX2 =  0;
                        offsetY2 *= 2;
                        break;
                    case PreserveAspectRatioAlign.xMidYMax:
                        offsetY2 *= 2;
                        break;
                    case PreserveAspectRatioAlign.xMaxYMax:
                        offsetX2 *= 2;
                        offsetY2 *= 2;
                        break;
                    case PreserveAspectRatioAlign.none:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                // If the BorderThickness should be taken into account, uncomment it.
                //using (dc.PushPostTransform(Matrix.CreateTranslation(-RenderBounds.Left, -RenderBounds.Top)))
                using (dc.PushPostTransform(Matrix.CreateScale(scale2, scale2)))
                using (dc.PushPostTransform(Matrix.CreateTranslation(offsetX2, offsetY2)))
                using (dc.PushTransformContainer())
                    this.Children?.RenderRecursively(dc);
            }

            // Draw view box border.
            dc.DrawRectangle(new Pen(Brushes.Purple, 1), new Rect(viewBox.Origin.X, viewBox.Origin.Y, viewBox.Width, viewBox.Height));
        }
    }

    private static void GetFillFactors(Size parentSize, Size childSize, out double scaleX, out double scaleY)
    {
        scaleX = parentSize.Width  / childSize.Width;
        scaleY = parentSize.Height / childSize.Height;
    }

    private static void GetUniformFactors(Size parentSize, Size childSize, bool fit, out double scale, out double offsetX, out double offsetY)
    {
        var scaleX = parentSize.Width  / childSize.Width;
        var scaleY = parentSize.Height / childSize.Height;
        scale   = fit ? Math.Max(scaleX, scaleY) : Math.Min(scaleX, scaleY);
        offsetX = (parentSize.Width  - childSize.Width  * scale) / 2;
        offsetY = (parentSize.Height - childSize.Height * scale) / 2;
    }

    Size ISvg.GetDesiredSize(Size availableSize)
    {
        var width = Width ?? double.NaN;
        var height = Height ?? double.NaN;

        if (double.IsNaN(width) || double.IsInfinity(width))
        {
            if (double.IsInfinity(availableSize.Width))
            {
                return Size.Empty;
            }
            else
            {
                width = availableSize.Width;
            }
        }
        if (double.IsNaN(height) || double.IsInfinity(height))
        {
            if (double.IsInfinity(height))
            {
                return Size.Empty;
            }
            else
            {
                height = availableSize.Height;
            }
        }

        return new Size(width, height);
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