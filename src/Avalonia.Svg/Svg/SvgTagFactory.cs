using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Avalonia.Svg;

/// <summary>
/// Svg static factory for building svg and svg tags.
/// </summary>
public static class SvgTagFactory
{
    internal static readonly Dictionary<SvgTags, ISvgTagFactory> SvgTagFactories;

    /// <summary>
    /// Pre-load all svg tag factories.
    /// </summary>
    static SvgTagFactory()
    {
        // TODO We can create the factories in need to save memory and initialization time.
        // Also we can pre-create the most popular factories to improve performance.

        SvgTagFactories = typeof(SvgTagFactory).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ISvgTagFactory)))
            .Select(t => (t.GetCustomAttribute<SvgTagAttribute>(), t))
            .Where(tuple => tuple.Item1 != null)
            .Select(tuple => (tuple.Item1, Activator.CreateInstance(tuple.t) as ISvgTagFactory))
            .ToDictionary(tuple => tuple.Item1!.Tag, tuple => tuple.Item2)!;
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

        var node = doc.FirstChild;
        if (node == null)
        {
            throw new InvalidDataException("Svg data has more than one root nodes which is invalid for svg file");
        }
        if (node.Name != SvgTags.svg.ToString())
        {
            throw new InvalidDataException($"First node of svg data should be {nameof(SvgTags.svg)} but {node.Name}");
        }

        var svg = CreateTagFrom(node);

        // Collect svg resources.
        if (svg is ISvgResourceCollector collector)
        {
            collector.CollectResources();
            svg.ApplyResources(collector);
        }

        return (svg as ISvg)!;
    }

    private static ISvgTag? CreateTagFrom(XmlNode node)
    {
        if (!Enum.TryParse<SvgTags>(node.LocalName.Replace('-', '_'), out var tagDefinition))
        {
            throw new Exception($"Can not recognize the tag named{node.LocalName}");
        }

        if (!SvgTagFactories.TryGetValue(tagDefinition, out var factory))
        {
            throw new Exception($"Tag named{node.LocalName} does not implement");
        }

        var tag = factory.CreateTag(node);

        if (!node.HasChildNodes)
        {
            return tag;
        }

        var childTags = node.ChildNodes
            .OfType<XmlNode>()
            .Where(n => n.NodeType == XmlNodeType.Element)
            .Select(CreateTagFrom)
            .OfType<ISvgTag>()
            .ToList();

        tag.Children = childTags;

        return tag;
    }
}
