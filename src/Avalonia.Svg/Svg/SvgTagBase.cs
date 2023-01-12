﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Avalonia.Svg;

/// <summary>
/// Abstract base class for svg tags. This class provides the shared method <see cref="GetTagName"/> and the ability to add deferred properties.
/// </summary>
public abstract class SvgTagBase : ISvgTag
{
    #region ISvgTag

    private string? _tagName;

    public  List<ISvgTag>? Children { get; set; }

    public virtual void ApplyResources(ISvgResourceCollector collector)
    {

    }

    public string GetTagName()
    {
        if (_tagName != null)
        {
            return _tagName;
        }

        var factoryType = this.GetType().Assembly.GetType($"{this.GetType().FullName}Factory");
        if (factoryType == null)
        {
            throw new Exception($"Can not find the factory for {GetType()}");
        }
        var attribute = factoryType.GetCustomAttribute<SvgTagAttribute>();
        if (attribute == null)
        {
            throw new InvalidOperationException($"Can not find the {nameof(SvgTagAttribute)} from type of {GetType()}");
        }

        _tagName = attribute.Tag.ToString();
        return _tagName;
    }

    #endregion



    #region Deferred Property

    protected Dictionary<string, string>? DeferredProperties;

    public void AddDeferred(string property, string valueString)
    {
        if (DeferredProperties == null)
        {
            DeferredProperties = new Dictionary<string, string>();
        }

        DeferredProperties.TryAdd(property, valueString);
    }

    #endregion
}
