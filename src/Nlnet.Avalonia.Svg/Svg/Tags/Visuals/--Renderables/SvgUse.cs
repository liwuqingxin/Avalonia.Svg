using System.Collections.Generic;
using System.Xml;
using Avalonia;
using Avalonia.Media;
using Nlnet.Avalonia.Svg.CompileGenerator;

namespace Nlnet.Avalonia.Svg;

[TagFactoryGenerator(nameof(SvgTags.use))]
public class SvgUse : SvgRenderable, ISvgRenderable, 
    IHrefSetter,
    IXHrefSetter,
    IXSetter, 
    IYSetter, 
    IWidthSetter, 
    IHeightSetter
{
    public string? Href
    {
        get;
        set;
    }
    public string? XHref
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

    public ISvgRenderable? ReferencedElement { get; set; }

    public SvgUse()
    {
        this.TryAddApplier(UseReferencedElementApplier.Instance);
    }

    protected override void RenderCore(DrawingContext dc, ISvgContext ctx)
    {
        if (ReferencedElement == null || Width == 0 || Height == 0)
        {
            return;
        }

        var matrix = Matrix.CreateTranslation(X ?? 0, Y ?? 0);
        if (Transform != null)
        {
            matrix = Transform.Value * matrix;
        }

        using (dc.PushTransform(matrix))
        {
            //using (dc.PushTransformContainer())
            {
                ReferencedElement.Render(dc, ctx);
            }
        }
    }
}
