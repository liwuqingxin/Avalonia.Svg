namespace Nlnet.Avalonia.Svg;

public enum RefXMode
{
    left       = 0,
    center     = 1, 
    right      = 2,
    number     = 3,
    percentage = 4,
}

public class RefX
{
    public static RefX Default { get; } = new();

    public RefXMode Mode { get; set; } = RefXMode.number;

    public double Value { get; set; }
}
