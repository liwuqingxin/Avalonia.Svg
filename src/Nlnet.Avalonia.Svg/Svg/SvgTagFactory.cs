﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Nlnet.Avalonia.Svg;

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
        
        var svgTag = CreateTagFrom(node);
        var svg = (Svg)svgTag;

        // Collect, build and apply svg resources.
        svg.PrepareContext();
        svg.BuildContext();
        svg.ApplyContext(svg);

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
                return Svg.Empty;
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
                return Svg.Empty;
            }
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
            .Where(t => t != Svg.Empty)
            .ToList();

        tag.Children = childTags;
        childTags.ForEach(t => t.Parent = tag);

        return tag;
    }
}
