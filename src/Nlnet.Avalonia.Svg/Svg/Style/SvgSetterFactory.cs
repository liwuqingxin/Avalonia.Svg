using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg static factory for building svg style setter.
/// </summary>
public static class SvgSetterFactory
{
    internal static readonly Dictionary<string, ISvgSetterFactory> SvgSetterFactories;

    /// <summary>
    /// Pre-load all svg style setter factories.
    /// </summary>
    static SvgSetterFactory()
    {
        // TODO We can create the factories in need to save memory and initialization time.
        // Also we can pre-create the most popular factories to improve performance.

        SvgSetterFactories = typeof(SvgSetterFactory).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ISvgSetterFactory)))
            .Select(t => (attr:t.GetCustomAttribute<NameAttribute>(), facType:t))
            .Where(tuple => tuple.attr != null)
            .Select(tuple => (tuple.attr, fac:Activator.CreateInstance(tuple.facType) as ISvgSetterFactory))
            .ToDictionary(tuple => tuple.attr!.Name, tuple => tuple.fac)!;
    }

    /// <summary>
    /// Get setter factory from name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static ISvgSetterFactory? GetSetterFactory(string name)
    {
        if (SvgSetterFactories.TryGetValue(name, out var factory))
        {
            return factory;
        }
        else
        {
            if (SvgParameters.Default.ThrowExceptionWhenAttributeNotImplement)
            {
                throw new NotImplementedException($"Attribute named '{name}' does not implement");
            }
            else
            {
                return null;
            }
        }
    }
}
