using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nlnet.Avalonia.Svg;

/// <summary>
/// Abstract base class for svg tags.
/// </summary>
public abstract class SvgTagBase : ISvgTag, IDeferredAdder
{
    #region Svg tag properties

    string? IClassSetter.Class { get; set; }

    string? IIdSetter.Id { get; set; }

    #endregion



    #region ISvgTag

    private string? _tagName;

    IReadOnlyDictionary<string, string>? ISvgTag.DeferredProperties => _deferredProperties;
    
    public ISvgTag? Parent { get; set; }

    public List<ISvgTag>? Children { get; set; }

    string ISvgTag.GetTagName()
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

    public virtual void ApplyContext(ISvgContext context)
    {
        if (_contextAppliers == null)
        {
            return;
        }

        foreach (var applier in _contextAppliers)
        {
            applier.Apply(this, context);
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



    #region ISvgContextApplier

    private IEnumerable<ISvgContextApplier>? _contextAppliers;

    protected void TryAddApplier(ISvgContextApplier applier)
    {
        _contextAppliers ??= new List<ISvgContextApplier>();
        ((List<ISvgContextApplier>)_contextAppliers).Add(applier);
    }

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

            var defaultValue = owner.DefaultValue as TValue;

            return owner.CanInherit
                ? this.Parent?.GetPropertyValue<TPropertyOwner, TValue>() ?? defaultValue
                : defaultValue;

        }
        else
        {
            return null;
            throw new InvalidOperationException($"Can not find the property implementation of {typeof(TPropertyOwner)} in tag type {this.GetType()}");
        }
    }

    public TValue GetPropertyStructValue<TPropertyOwner, TValue>()
        where TPropertyOwner : class, ISvgProperty<TPropertyOwner>
        where TValue : struct
    {
        if (this is TPropertyOwner owner)
        {
            if (owner.Value is TValue value)
            {
                return value;
            }

            if (owner.DefaultValue is TValue defaultValue)
            {
                if (this.Parent is TPropertyOwner && owner.CanInherit)
                {
                    return this.Parent.GetPropertyStructValue<TPropertyOwner, TValue>();
                }

                return defaultValue;
            }
            else
            {
                throw new Exception($"The default value for struct type {typeof(TValue)} should not be null or {owner.DefaultValue}");
            }
        }
        else
        {
            throw new InvalidOperationException($"Can not find the property implementation of {typeof(TPropertyOwner)} in tag type {this.GetType()}");
        }
    }

    #endregion
}