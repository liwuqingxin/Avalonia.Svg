using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using System.Xml;

namespace Nlnet.Avalonia.Svg;

public interface IPropertyFetcher
{
    public void Fetch(ISvgTag target, XmlAttributeCollection? attrs);
}

public class DefaultPropertyFetcher : IPropertyFetcher
{
    private readonly ConcurrentDictionary<Type, List<MethodInfo>> _setterCaches = new();

    public void Fetch(ISvgTag target, XmlAttributeCollection? attrs)
    {
        // TODO For convenience here, temporarily use reflection to parse the implemented attribute interface, and then consider performance later.

        if (attrs == null)
        {
            return;
        }

        if (!_setterCaches.TryGetValue(target.GetType(), out var list))
        {
            list = target
                .GetType()
                .GetInterfaces()
                .Where(it => it.IsAssignableTo(typeof(IDeferredAdder)))
                .Select(type => type.GetMethod(type.Name))
                .Where(m => m != null)
                .ToList()!;
            _setterCaches.TryAdd(target.GetType(), list);
        }

        foreach (var parser in list)
        {
            parser.Invoke(target, new object?[] { attrs });
        }
    }
}
