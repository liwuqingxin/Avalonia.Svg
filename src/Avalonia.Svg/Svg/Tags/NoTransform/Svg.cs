using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Avalonia.Media;

namespace Avalonia.Svg;

[SvgTag(SvgTags.svg)]
public class SvgFactory : ISvgTagFactory
{
    public ISvgTag CreateTag(XmlNode xmlNode)
    {
        var tag = new Svg();

        if (xmlNode.Attributes != null)
        {
            tag.Id      = xmlNode.Attributes["id"]?.Value;
            tag.Version = xmlNode.Attributes["version"]?.Value;
            tag.Style   = xmlNode.Attributes["style"]?.Value;
            tag.ViewBox = xmlNode.Attributes["viewBox"]?.Value;
            tag.Y       = xmlNode.Attributes["y"]?.Value;
            tag.X       = xmlNode.Attributes["x"]?.Value;
        }

        return tag;
    }
}

public class Svg : SvgTagBase, ISvg, ISvgResourceCollector
{
    public string? Id      { get; set; }
    public string? Version { get; set; }
    public string? Style   { get; set; }
    public string? ViewBox { get; set; }
    public string? Y       { get; set; }
    public string? X       { get; set; }

    private IList<ISvgStyleProvider> StyleProviders { get; set; } = new List<ISvgStyleProvider>();

    private IList<ISvgBrushProvider> BrushProviders { get; set; } = new List<ISvgBrushProvider>();

    private Dictionary<string, ISvgClassStyle> Styles { get; set; } = new Dictionary<string, ISvgClassStyle>();

    private Dictionary<string, IBrush> Brushes { get; set; } = new Dictionary<string, IBrush>();

    private List<ISvgVisual> Visuals { get; set; } = new List<ISvgVisual>();



    #region ISvgResourceCollector

    IList<ISvgStyleProvider> ISvgResourceCollector.StyleProviders => StyleProviders;

    IList<ISvgBrushProvider> ISvgResourceCollector.BrushProviders => BrushProviders;

    IReadOnlyDictionary<string, ISvgClassStyle> ISvgResourceCollector.Styles => this.Styles;

    IReadOnlyDictionary<string, IBrush> ISvgResourceCollector.Brushes => this.Brushes;

    IReadOnlyList<ISvgVisual> ISvgResourceCollector.Visuals => this.Visuals;

    void ISvgResourceCollector.CollectResources()
    {
        CollectResources(this, this);
    }

    private static void CollectResources(ISvgTag tag, Svg svg)
    {
        if (tag is ISvgStyleProvider styleProvider)
        {
            svg.StyleProviders.Add(styleProvider);
            foreach (var style in styleProvider.GetStyles())
            {
                svg.Styles.Add(style.Class, style);
            }
        }

        if (tag is ISvgBrushProvider brushProvider)
        {
            svg.BrushProviders.Add(brushProvider);
            svg.Brushes.Add(brushProvider.Id, brushProvider.GetBrush());
        }

        if (tag is ISvgVisual visual)
        {
            svg.Visuals.Add(visual);
        }

        if (tag.Children == null)
        {
            return;
        }

        foreach (var child in tag.Children)
        {
            CollectResources(child, svg);
        }
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
            ApplyResources(collector, child);
        }

        var transform = SvgHelper.GetAlignToTopLeftTransform(Visuals.Select(v => v.Bounds));
        foreach (var visual in Visuals)
        {
            visual.ApplyTransform(transform);
        }
    }

    private static void ApplyResources(ISvgResourceCollector collector, ISvgTag tag)
    {
        tag.ApplyResources(collector);

        if (tag.Children == null)
        {
            return;
        }

        foreach (var child in tag.Children)
        {
            ApplyResources(collector, child);
        }
    }

    void ISvg.Render(DrawingContext dc)
    {
        foreach (var visual in Visuals)
        {
            visual.Render(dc);
        }
    }

    Size ISvg.GetRenderSize()
    {
        var left   = 0d;
        var top    = 0d;
        var right  = 0d;
        var bottom = 0d;

        foreach (var visual in Visuals)
        {
            left   = Math.Min(visual.RenderBounds.Left, left);
            top    = Math.Min(visual.RenderBounds.Top, top);
            right  = Math.Max(visual.RenderBounds.Right, right);
            bottom = Math.Max(visual.RenderBounds.Bottom, bottom);
        }

        return new Size(right - left, bottom - top);
    }
}