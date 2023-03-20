using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.svg))]
public class Svg : SvgContainer, ISvg, ISvgContext, ISvgContainer, ISvgRenderable, IInitializable,
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

    private readonly List<ISvgStyle>                _styles      = new();
    private readonly Dictionary<string, LightBrush> _brushes     = new();
    private readonly Dictionary<string, ISvgTag>    _idTags      = new();
    private readonly List<ISvgRenderable>           _renderables = new();



    #region ISvgContext

    IReadOnlyList<ISvgStyle> ISvgContext.Styles => this._styles;

    IReadOnlyDictionary<string, LightBrush> ISvgContext.Brushes => this._brushes;

    IReadOnlyDictionary<string, ISvgTag> ISvgContext.IdTags => this._idTags;

    IReadOnlyList<ISvgRenderable> ISvgContext.Renderables => this._renderables;

    #endregion



    #region ISvg

    void ISvg.Render(DrawingContext dc, Size availableSize, bool showDiagnosis)
    {
        var viewBox     = ViewBox ?? new ViewBox(RenderBounds.Left, RenderBounds.Top, RenderBounds.Width, RenderBounds.Height);
        var viewBoxSize = new Size(viewBox.Width, viewBox.Height);
        var ratio       = PreserveAspectRatio ?? new PreserveAspectRatio(PreserveAspectRatioAlign.xMidYMid, PreserveAspectRatioMeetOrSlice.meet);
        if (ratio.Align == PreserveAspectRatioAlign.none)
        {
            GetFillFactors(availableSize, viewBoxSize, out var scaleX, out var scaleY);
            using (dc.PushPostTransform(Matrix.CreateTranslation(-viewBox.Origin.X, -viewBox.Origin.Y)))
            using (dc.PushPostTransform(Matrix.CreateScale(scaleX, scaleY)))
            using (dc.PushTransformContainer())
                this.Children?.RenderRecursively(dc);
        }
        else
        {
            var isSlice = ratio.MeetOrSlice == PreserveAspectRatioMeetOrSlice.slice;
            GetUniformFactors(availableSize, viewBoxSize, isSlice, out var scale, out var offsetX, out var offsetY);
            switch (ratio.Align)
            {
                case PreserveAspectRatioAlign.xMinYMin:
                    offsetX = 0;
                    offsetY = 0;
                    break;
                case PreserveAspectRatioAlign.xMidYMin:
                    offsetY = 0;
                    break;
                case PreserveAspectRatioAlign.xMaxYMin:
                    offsetX *= 2;
                    offsetY = 0;
                    break;
                case PreserveAspectRatioAlign.xMinYMid:
                    offsetX = 0;
                    break;
                case PreserveAspectRatioAlign.xMidYMid:
                    break;
                case PreserveAspectRatioAlign.xMaxYMid:
                    offsetX *= 2;
                    break;
                case PreserveAspectRatioAlign.xMinYMax:
                    offsetX = 0;
                    offsetY *= 2;
                    break;
                case PreserveAspectRatioAlign.xMidYMax:
                    offsetY *= 2;
                    break;
                case PreserveAspectRatioAlign.xMaxYMax:
                    offsetX *= 2;
                    offsetY *= 2;
                    break;
                case PreserveAspectRatioAlign.none:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            using (dc.PushPostTransform(Matrix.CreateTranslation(-viewBox.Origin.X, -viewBox.Origin.Y)))
            using (dc.PushPostTransform(Matrix.CreateScale(scale, scale)))
            using (dc.PushPostTransform(Matrix.CreateTranslation(offsetX, offsetY)))
            using (dc.PushTransformContainer())
                this.Children?.RenderRecursively(dc);
        }

        // Draw view box border.
        if (showDiagnosis)
        {
            dc.DrawRectangle(new Pen(Brushes.Green, 1, new DashStyle(new double[] { 5, 5 }, 0)), new Rect(viewBox.Origin.X, viewBox.Origin.Y, viewBox.Width, viewBox.Height));
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



    private void PrepareContext()
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
                    this._styles.Add(style);
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

    private void BuildContext()
    {
        foreach (var setter in _styles.SelectMany(style => style.Setters))
        {
            setter.ApplyDeferredValueString(this);
        }
    }

    public override void ApplyContext(ISvgContext context)
    {
        if (Children == null)
        {
            return;
        }

        this.VisitSvgTagTreeWithoutSelf(tag =>
        {
            tag.ApplyContext(context);
        });
    }

    void IInitializable.Initialize()
    {
        // Collect, build and apply svg context.
        this.PrepareContext();
        this.BuildContext();
        this.ApplyContext(this);

        // Apply transforms.
        this.ApplyTransforms(new Stack<Matrix>());
    }
}