﻿using System;

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

    public double Get(double height)
    {
        return Mode switch
        {
            RefYMode.top => 0,
            RefYMode.center => height / 2,
            RefYMode.bottom => height,
            RefYMode.number => Value,
            RefYMode.percentage => Value * height,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

