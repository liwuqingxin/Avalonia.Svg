using System;
using System.Xml;

namespace Nlnet.Avalonia.Svg
{
    internal static class PropertyHelper
    {
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
