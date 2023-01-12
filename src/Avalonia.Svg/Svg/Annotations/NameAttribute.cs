using System;

namespace Avalonia.Svg;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
internal sealed class NameAttribute : Attribute
{
    public string Name { get; set; }

    public NameAttribute(string name)
    {
        Name = name;
    }
}
