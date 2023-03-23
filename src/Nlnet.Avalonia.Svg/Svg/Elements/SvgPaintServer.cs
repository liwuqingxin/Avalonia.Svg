using System;

namespace Nlnet.Avalonia.Svg;

public abstract class SvgPaintServer : SvgTagBase, ISvgPaintServer
{
    private bool? _templateApplied = false;

    protected abstract string? Href { get; }

    internal void EnsureTemplate(ISvgContext context)
    {
        if (_templateApplied == null)
        {
            throw new Exception($"Got a circular references in {this.GetType().Name} by href");
        }
        if (_templateApplied == true || string.IsNullOrEmpty(Href))
        {
            return;
        }

        _templateApplied = null;

        if (Href.TryParseHref(out var id) == false)
        {
            // Never try to apply template if href format error.
            _templateApplied = true;
            return;
        }
        if (context.IdTags.TryGetValue(id, out var template) == false)
        {
            // Never try to apply template again if the target template is not found.
            _templateApplied = true;
            return;
        }
        if (template.GetType() != this.GetType())
        {
            // Never try to apply template if wrong template type.
            _templateApplied = true;
            return;
        }

        var paintServer = (SvgPaintServer) template;
        paintServer.EnsureTemplate(context);
        EnsureTemplateCore(context, paintServer);

        _templateApplied = true;
    }

    protected abstract void EnsureTemplateCore(ISvgContext context, SvgPaintServer paintServer);

    protected SvgPaintServer()
    {
        this.TryAddApplier(new PaintServerTemplateApplier());
    }
}
