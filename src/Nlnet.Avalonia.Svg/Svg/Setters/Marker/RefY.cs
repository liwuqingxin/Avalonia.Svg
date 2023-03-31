namespace Nlnet.Avalonia.Svg;

public enum RefYMode
{
    top        = 0,
    center     = 1,
    bottom     = 2,
    number     = 3,
    percentage = 4,
}

public class RefY
{
    public static RefY Default { get; } = new();

    public RefYMode Mode { get; set; } = RefYMode.number;

    public double Value { get; set; }
}

