using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Avalonia.Svg.Setters;

namespace Avalonia.Svg
{
    internal static class PropertyService
    {
        private static readonly ConcurrentDictionary<Type, List<MethodInfo>> SetterTypeCaches = new();

        public static void FetchProperties(this ISvgTag target, XmlAttributeCollection attrs)
        {
            // TODO For convenience here, temporarily use reflection to parse the implemented attribute interface, and then consider performance later.

            if (!SetterTypeCaches.TryGetValue(target.GetType(), out var list))
            {
                list = target
                    .GetType()
                    .GetInterfaces()
                    .Where(it => it.IsAssignableTo(typeof(IDeferredAdder)))
                    .Select(type => type.GetMethod("Parse"))
                    .Where(m => m != null)
                    .ToList()!;
                SetterTypeCaches.TryAdd(target.GetType(), list);
            }

            foreach (var method in list)
            {
                method.Invoke(target, new object?[] {attrs});
            }
        }

        /// <summary>
        /// Try-convert delegate.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="valueString"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public delegate bool TryConverter<TValue>(string valueString, out TValue value);

        /// <summary>
        /// Try parse value from value string or defer to set it.
        /// </summary>
        /// <typeparam name="TTarget">Target type like <see cref="IFillSetter"/></typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="target"></param>
        /// <param name="attributes"></param>
        /// <param name="property"></param>
        /// <param name="converter"></param>
        /// <param name="setter"></param>
        public static void ParseOrDefer<TTarget, TValue>(
            this TTarget            target,
            XmlAttributeCollection  attributes,
            string                  property,
            TryConverter<TValue>    converter,
            Action<TTarget, TValue> setter)
            where TTarget : IDeferredAdder
        {
            var valueString = attributes[property]?.Value;
            if (string.IsNullOrWhiteSpace(valueString))
            {
                return;
            }

            if (converter.Invoke(valueString, out var value))
            {
                setter.Invoke(target, value);
            }
            else
            {
                target.AddDeferred(property, valueString);
            }
        }
    }
}
