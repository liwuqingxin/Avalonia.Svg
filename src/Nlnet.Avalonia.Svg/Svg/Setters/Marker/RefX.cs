using System;

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

    public double Get(double width)
    {
        return Mode switch
        {
            RefXMode.left => 0,
            RefXMode.center => width / 2,
            RefXMode.right => width,
            RefXMode.number => Value,
            RefXMode.percentage => Value * width,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
