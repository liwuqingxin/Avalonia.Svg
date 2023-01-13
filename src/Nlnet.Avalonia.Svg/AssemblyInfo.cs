using System.Runtime.CompilerServices;
using Avalonia.Metadata;

[assembly: InternalsVisibleTo("Nlnet.Avalonia.Svg.Test")]

// 不需要修改
[assembly: XmlnsPrefix("https://nlnet.com/avalonia.svg", "nlnet")]

// 添加新的命名空间在此添加对应代码行
[assembly: XmlnsDefinition("https://nlnet.com/avalonia.svg", "Nlnet.Avalonia.Svg")]
[assembly: XmlnsDefinition("https://nlnet.com/avalonia.svg", "Nlnet.Avalonia.Svg.Controls")]
[assembly: XmlnsDefinition("https://nlnet.com/avalonia.svg", "Nlnet.Avalonia.Svg.Utils")]
