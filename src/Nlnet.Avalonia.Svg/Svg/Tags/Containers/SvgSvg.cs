using System;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;
using Nlnet.Avalonia.Svg.Utils;

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

        if (Children == null || this.IsRoot == false)
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
        using var stack = new StateStack();

        stack.Push(dc.PushTransform(Matrix.CreateTranslation(X ?? 0, Y ?? 0)));
        //stack.Push(dc.PushTransformContainer());

        this.PushViewBox(stack, dc, availableSize);
        //stack.Push(dc.PushTransformContainer());

        RenderChildren(dc, ctx);

        // Draw view box border.
        if (ctx.ShowDiagnosis && ViewBox != null)
        {
            dc.DrawRectangle(new Pen(Brushes.Green, 1, new DashStyle(new double[] { 5, 5 }, 0)), ViewBox.Bounds);
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

        using (dc.PushTransform(matrix))
            //using (dc.PushTransformContainer())
            foreach (var child in Children.OfType<ISvgRenderable>())
            {
                child.Render(dc, ctx);
            }
    }
}