using System.Linq;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Use referenced element applier for <see cref="ISvgContextApplier"/>.
/// </summary>
public class UseReferencedElementApplier : ISvgContextApplier
{
    public static UseReferencedElementApplier Instance { get; } = new();

    public void Apply(ISvgTag tag, ISvgContext context)
    {
        if (tag is SvgUse use)
        {
            var href = use.Href ?? use.XHref;
            if (href == null)
            {
                return;
            }

            if (!href.TryParseHref(out var id))
            {
                return;
            }

            if (context.IdTags.TryGetValue(id, out var element))
            {
                use.ReferencedElement = element as ISvgRenderable;

                if (tag.Style != null && use.ReferencedElement != null)
                {
                    tag.Style.ApplyTo(use.ReferencedElement);
                }
            }
        }
    }
}