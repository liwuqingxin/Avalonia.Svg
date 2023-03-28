using Nlnet.Avalonia.Svg.CompileGenerator;
// ReSharper disable InconsistentNaming

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(typeof(double), SvgDefaultValues.Zero, false)]
public class X1Setter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IX1Setter setter)
        {
            return;
        }

        setter.X1 = Value;
    }
}

[SetterGenerator(typeof(double), SvgDefaultValues.Zero, false)]
public class Y1Setter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IY1Setter setter)
        {
            return;
        }

        setter.Y1 = Value;
    }
}

[SetterGenerator(typeof(double), SvgDefaultValues.One, false)]
public class X2Setter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IX2Setter setter)
        {
            return;
        }

        setter.X2 = Value;
    }
}

[SetterGenerator(typeof(double), SvgDefaultValues.Zero, false)]
public class Y2Setter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IY2Setter setter)
        {
            return;
        }

        setter.Y2 = Value;
    }
}