using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Svg static factory for building svg style setter.
/// </summary>
public static class SvgStyleSetterFactory
{
    internal static readonly Dictionary<string, ISvgStyleSetterFactory> SvgSetterFactories;

    /// <summary>
    /// Pre-load all svg style setter factories.
    /// </summary>
    static SvgStyleSetterFactory()
    {
        // TODO We can create the factories in need to save memory and initialization time.
        // Also we can pre-create the most popular factories to improve performance.

        SvgSetterFactories = typeof(SvgStyleSetterFactory).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ISvgStyleSetterFactory)))
            .Select(t => (t.GetCustomAttribute<NameAttribute>(), t))
            .Where(tuple => tuple.Item1 != null)
            .Select(tuple => (tuple.Item1, Activator.CreateInstance(tuple.t) as ISvgStyleSetterFactory))
            .ToDictionary(tuple => tuple.Item1!.Name, tuple => tuple.Item2)!;
    }

    /// <summary>
    /// Get setter factory from name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static ISvgStyleSetterFactory? GetSetterFactory(string name)
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
