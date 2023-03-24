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

    private Matrix? _matrix;

    public SvgUse()
    {
        this.TryAddApplier(UseReferencedElementApplier.Instance);
    }

    public override void ApplyTransforms(Stack<Matrix> transformsContext)
    {
        if (ReferencedElement == null || Width == 0 || Height == 0)
        {
            return;
        }

        _matrix = Matrix.CreateTranslation(X ?? 0, Y ?? 0);
        if (Transform != null)
        {
            _matrix = _matrix * Transform.Value;
        }
        if (transformsContext.TryPeek(out var containerMatrix))
        {
            _matrix = _matrix * containerMatrix;
        }
    }

    public override void Render(DrawingContext dc)
    {
        if (ReferencedElement == null || Width == 0 || Height == 0 || _matrix == null)
        {
            return;
        }

        using (dc.PushPostTransform(_matrix.Value))
        {
            using (dc.PushTransformContainer())
            {
                ReferencedElement.Render(dc);
            }
        }
    }
}
