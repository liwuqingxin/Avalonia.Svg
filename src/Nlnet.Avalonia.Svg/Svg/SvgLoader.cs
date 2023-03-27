using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Avalonia;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg loader for building svg and svg tags.
/// </summary>
public static class SvgLoader
{
    internal static readonly Dictionary<SvgTags, ISvgTagFactory> SvgTagFactories;

    /// <summary>
    /// Pre-load all svg tag factories.
    /// </summary>
    static SvgLoader()
    {
        // TODO We can create the factories in need to save memory and initialization time.
        // Also we can pre-create the most popular factories to improve performance.

        SvgTagFactories = typeof(SvgLoader).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ISvgTagFactory)))
            .Select(t => (attr:t.GetCustomAttribute<SvgTagAttribute>(), facType:t))
            .Where(tuple => tuple.attr != null)
            .Select(tuple => (tuple.attr, fac:Activator.CreateInstance(tuple.facType) as ISvgTagFactory))
            .ToDictionary(tuple => tuple.attr!.Tag, tuple => tuple.fac)!;
    }

    /// <summary>
    /// Load svg from a file.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public static ISvg LoadSvgFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException(filename);
        }

        var svgData = File.ReadAllText(filename);
        return LoadSvg(svgData);
    }

    /// <summary>
    /// Load svg from svg document data.
    /// </summary>
    /// <param name="svgData"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static ISvg LoadSvg(string svgData)
    {
        var doc = new XmlDocument();
        doc.LoadXml(svgData);

        var node = doc.ChildNodes.OfType<XmlNode>().FirstOrDefault(n => n.NodeType == XmlNodeType.Element && n.Name == SvgTags.svg.ToString());
        if (node == null)
        {
            throw new InvalidDataException("Can not find the svg tag in the svg document");
        }

        var svgTag = (SvgSvg)CreateTagFrom(node);
        svgTag.IsRoot = true;

        var svg = new Svg(svgTag);
        svg.Initialize();
        
        return svg;
    }

    private static ISvgTag CreateTagFrom(XmlNode node)
    {
        if (!Enum.TryParse<SvgTags>(node.LocalName.Replace('-', '_'), out var tagDefinition))
        {
            if (SvgParameters.Default.ThrowExceptionWhenTagNotRecognized)
            {
                throw new Exception($"Can not recognize the tag named{node.LocalName}");
            }
            else
            {
                return SvgSvg.Empty;
            }
        }

        if (!SvgTagFactories.TryGetValue(tagDefinition, out var factory))
        {
            if (SvgParameters.Default.ThrowExceptionWhenTagNotImplement)
            {
                throw new NotImplementedException($"Tag named '{node.LocalName}' does not implement");
            }
            else
            {
                return SvgSvg.Empty;
            }
        }

        var tag = factory.CreateTag(node);

        if (tag is SvgDefs)
        {
            tag.IsDef = true;
        }

        if (!node.HasChildNodes)
        {
            return tag;
        }

        var childTags = node.ChildNodes
            .OfType<XmlNode>()
            .Where(n => n.NodeType == XmlNodeType.Element)
            .Select(CreateTagFrom)
            .Where(t => t != SvgSvg.Empty)
            .ToList();

        tag.Children = childTags;
        childTags.ForEach(t =>
        {
            t.Parent = tag;
            t.IsDef  = tag.IsDef;
        });

        return tag;
    }
}
