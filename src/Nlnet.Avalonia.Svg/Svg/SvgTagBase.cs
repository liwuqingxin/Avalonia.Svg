using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base class for svg tags.
/// </summary>
public abstract class SvgTagBase : ISvgTag, IDeferredAdder
{
    #region ISvgTag

    private string? _tagName;

    IReadOnlyDictionary<string, string>? ISvgTag.DeferredProperties => _deferredProperties;
    
    public ISvgTag? Parent { get; set; }

    public List<ISvgTag>? Children { get; set; }

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

    public virtual void ApplyResources(ISvgResourceCollector collector)
    {
        if (ResourceAppliers == null)
        {
            return;
        }

        foreach (var applier in ResourceAppliers)
        {
            applier.Apply(this, collector);
        }
    }

    public virtual void OnPropertiesFetched()
    {
        
    }

    #endregion



    #region Deferred Property

    private Dictionary<string, string>? _deferredProperties;

    public void AddDeferred(string property, string valueString)
    {
        _deferredProperties ??= new Dictionary<string, string>();
        _deferredProperties.TryAdd(property, valueString);
    }

    #endregion



    #region ISvgResourceApplier

    protected List<ISvgResourceApplier>? ResourceAppliers { get; init; }

    #endregion



    #region Inheritable Properties accessor

    public TValue? GetPropertyValue<TPropertyOwner, TValue>()
        where TPropertyOwner : class, ISvgProperty<TPropertyOwner>
        where TValue : class
    {
        if (this is TPropertyOwner owner)
        {
            if (owner.Value is TValue value)
            {
                return value;
            }

            return owner.CanInherit ? this.Parent?.GetPropertyValue<TPropertyOwner, TValue>() : null;

        }
        else
        {
            return null;
            throw new InvalidOperationException($"Can not find the property implementation of {typeof(TPropertyOwner)} in tag type {this.GetType()}");
        }
    }

    public TValue? GetPropertyStructValue<TPropertyOwner, TValue>()
        where TPropertyOwner : class, ISvgProperty<TPropertyOwner>
        where TValue : struct
    {
        if (this is TPropertyOwner owner)
        {
            if (owner.Value is TValue value)
            {
                return value;
            }

            return owner.CanInherit ? this.Parent?.GetPropertyStructValue<TPropertyOwner, TValue>() : null;

        }
        else
        {
            return null;
            throw new InvalidOperationException($"Can not find the property implementation of {typeof(TPropertyOwner)} in tag type {this.GetType()}");
        }
    }

    #endregion
}