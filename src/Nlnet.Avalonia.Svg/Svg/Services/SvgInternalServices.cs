namespace Nlnet.Avalonia.Svg;

internal static class SvgInternalServices
{
    public static IPropertyFetcher PropertyFetcher { get; set; } = new DefaultPropertyFetcher();
}
