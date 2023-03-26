using System.Linq;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Template properties applier for <see cref="SvgPaintServer"/>.
/// </summary>
public class PaintServerTemplateApplier : ISvgContextApplier
{
    public static PaintServerTemplateApplier Instance { get; } = new();

    public void Apply(ISvgTag tag, ISvgContext context)
    {
        if (tag is not SvgPaintServer paintServer)
        {
            return;
        }

        paintServer.EnsureTemplate(context);
    }
}