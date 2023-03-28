using System;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.svg))]
public class SvgSvg : SvgContainer, ISvgContainer, ISvgRenderable,
    //IVersionSetter,
    IViewBoxSetter,
    IPreserveAspectRatioSetter,
    IXSetter,
    IYSetter,
    IWidthSetter,
    IHeightSetter
{
    public static SvgSvg Empty { get; } = new();

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

    public bool IsRoot => Parent == null;



    public override void ApplyContext(ISvgContext context)
    {
        base.ApplyContext(context);

        if (Children == null)
        {
            return;
        }

        this.VisitSvgTagTreeWithoutSelf(tag =>
        {
            tag.ApplyContext(context);
        });
    }

    protected override void RenderCore(DrawingContext dc, ISvgContext ctx)
    {
        var availableSize = ctx.ContainerSize;

        if (this.Width != null)
        {
            availableSize = availableSize.WithWidth(this.Width.Value);
        }
        if (this.Height != null)
        {
            availableSize = availableSize.WithHeight(this.Height.Value);
        }

        if (IsRoot)
        {
            RenderCore(dc, ctx, availableSize);
        }
        else
        {
            using (dc.PushClip(new Rect(X ?? 0, Y ?? 0, availableSize.Width, availableSize.Height)))
            {
                RenderCore(dc, ctx, availableSize);
            }
        }
    }

    private void RenderCore(DrawingContext dc, ISvgContext ctx, Size availableSize)
    {
        if (this.ViewBox == null)
        {
            RenderChildren(dc, ctx);
        }
        else
        {
            var viewBoxSize = new Size(ViewBox.Width, ViewBox.Height);
            var ratio = PreserveAspectRatio ?? new PreserveAspectRatio(PreserveAspectRatioAlign.xMidYMid, PreserveAspectRatioMeetOrSlice.meet);
            if (ratio.Align == PreserveAspectRatioAlign.none)
            {
                SvgHelper.GetFillFactors(availableSize, viewBoxSize, out var scaleX, out var scaleY);
                using (dc.PushPostTransform(Matrix.CreateTranslation(-ViewBox.Origin.X, -ViewBox.Origin.Y)))
                using (dc.PushPostTransform(Matrix.CreateScale(scaleX, scaleY)))
                using (dc.PushPostTransform(Matrix.CreateTranslation(X ?? 0, Y ?? 0)))
                using (dc.PushTransformContainer())
                    RenderChildren(dc, ctx);
            }
            else
            {
                var isSlice = ratio.MeetOrSlice == PreserveAspectRatioMeetOrSlice.slice;
                SvgHelper.GetUniformFactors(availableSize, viewBoxSize, isSlice, out var scale, out var offsetX, out var offsetY);
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

                using (dc.PushPostTransform(Matrix.CreateTranslation(-ViewBox.Origin.X, -ViewBox.Origin.Y)))
                using (dc.PushPostTransform(Matrix.CreateScale(scale, scale)))
                using (dc.PushPostTransform(Matrix.CreateTranslation(offsetX, offsetY)))
                using (dc.PushPostTransform(Matrix.CreateTranslation(X ?? 0, Y ?? 0)))
                using (dc.PushTransformContainer())
                    RenderChildren(dc, ctx);
            }

            // Draw view box border.
            if (ctx.ShowDiagnosis)
            {
                dc.DrawRectangle(new Pen(Brushes.Green, 1, new DashStyle(new double[] { 5, 5 }, 0)), new Rect(ViewBox.Origin.X, ViewBox.Origin.Y, ViewBox.Width, ViewBox.Height));
            }
        }
    }

    private void RenderChildren(DrawingContext dc, ISvgContext ctx)
    {
        if (this.Children == null)
        {
            return;
        }

        var matrix = Matrix.Identity;

        // Transform
        if (Transform != null)
        {
            matrix *= Transform.Value;
        }

        using (dc.PushPostTransform(matrix))
        {
            using (dc.PushTransformContainer())
            {
                foreach (var child in Children.OfType<ISvgRenderable>())
                {
                    child.Render(dc, ctx);
                }
            }
        }
    }
}