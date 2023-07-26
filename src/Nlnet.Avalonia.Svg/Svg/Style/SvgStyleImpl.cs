using Nlnet.Avalonia.Svg.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Default implementation of <see cref="ISvgStyle"/>.
/// </summary>
public class SvgStyleImpl : ISvgStyle
{
    private readonly IEnumerable<IStyleSelector>? _selectors;

    private SvgStyleImpl(IEnumerable<IStyleSelector>? selectors, IEnumerable<ISvgSetter> setters)
    {
        _selectors = selectors;
        Setters    = setters;
    }

    public IEnumerable<ISvgSetter> Setters { get; set; }

    void ISvgStyle.ApplyTo(ISvgTag tag)
    {
        foreach (var setter in Setters)
        {
            setter.Set(tag);
        }
    }

    public bool Match(ISvgTag tag)
    {
        if (_selectors == null)
        {
            return false;
        }

        return _selectors.All(s => s.Match(tag));
    }

    public static ISvgStyle? Parse(string? selectorsString, string? settersString)
    {
        if (settersString == null)
        {
            return null;
        }

        var selectors = GetSelectorsFromString(selectorsString);
        var setters   = GetSettersFromStyleString(settersString);
        if (selectors != null && setters != null)
        {
            return new SvgStyleImpl(selectors, setters);
        }

        return null;
    }

    private static IEnumerable<ISvgSetter>? GetSettersFromStyleString(string? styleString)
    {
        if (string.IsNullOrWhiteSpace(styleString))
        {
            return null;
        }

        var setters = new List<ISvgSetter>();
        var token = new SafeStringTokenizer(styleString, ';');
        while (true)
        {
            if (!token.TryReadString(out var setterString))
            {
                break;
            }
            if (string.IsNullOrWhiteSpace(setterString?.Trim()))
            {
                continue;
            }
            var name = setterString[..setterString.IndexOf(':')].Trim();
            var factory = SvgSetterFactory.GetSetterFactory(name);
            if (factory == null)
            {
                continue;
            }
            var setter = factory.CreateSetter();
            var valueString = setterString[(setterString.IndexOf(':') + 1)..].Trim();
            try
            {
                setter.InitializeValue(valueString);
            }
            catch
            {
                setter.AddDeferredValueString(valueString);
            }
            setters.Add(setter);
        }

        if (setters.Count == 0)
        {
            return null;
        }

        return setters;
    }

    private static IEnumerable<IStyleSelector>? GetSelectorsFromString(string? selectorString)
    {
        if (string.IsNullOrWhiteSpace(selectorString))
        {
            return null;
        }

        selectorString = selectorString.Trim();
        var selectors = new List<IStyleSelector>();

        for (var i = 0; i < selectorString.Length; i++)
        {
            var ch = selectorString[i];
            var selector = ReadSelector(selectorString, ref i);
            switch (ch)
            {
                case '.':
                    selectors.Add(new ClassSelector(selector));
                    break;
                case '#':
                    selectors.Add(new IdSelector(selector));
                    break;
                default:
                    selectors.Add(new TagSelector(selector));
                    break;
            }
        }

        if (selectors.Count == 0)
        {
            return null;
        }

        return selectors;
    }

    private static string ReadSelector(string selectorString, ref int i)
    {
        var start = i;
        i++;
        while (i < selectorString.Length)
        {
            var ch = selectorString[i];
            if (ch is '.' or '#')
            {
                break;
            }
            i++;
        }

        return selectorString.Substring(start, i - start);
    }
}
