using Avalonia.Styling;
using Nlnet.Avalonia.Svg.CompileGenerator;
// ReSharper disable InconsistentNaming

namespace Nlnet.Avalonia.Svg;

[SetterGenerator(nameof(SvgProperties.X), SvgTypes.Double, SvgDefaultValues.Zero, false)]
public class XSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IXSetter setter)
        {
            return;
        }

        setter.X = Value;
    }
}

[SetterGenerator(nameof(SvgProperties.Y), SvgTypes.Double, SvgDefaultValues.Zero, false)]
public class YSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IYSetter setter)
        {
            return;
        }

        setter.Y = Value;
    }
}

[SetterGenerator(nameof(SvgProperties.CX), SvgTypes.Double, SvgDefaultValues.Zero, false)]
public class CXSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not ICXSetter setter)
        {
            return;
        }

        setter.CX = Value;
    }
}

[SetterGenerator(nameof(SvgProperties.CY), SvgTypes.Double, SvgDefaultValues.Zero, false)]
public class CYSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not ICYSetter setter)
        {
            return;
        }

        setter.CY = Value;
    }
}

[SetterGenerator(nameof(SvgProperties.RX), SvgTypes.Double, SvgDefaultValues.Zero, false)]
public class RXSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IRXSetter setter)
        {
            return;
        }

        setter.RX = Value;
    }
}

[SetterGenerator(nameof(SvgProperties.RY), SvgTypes.Double, SvgDefaultValues.Zero, false)]
public class RYSetter : AbstractDoubleSetter
{
    public override void Set(ISvgTag tag)
    {
        if (tag is not IRYSetter setter)
        {
            return;
        }

        setter.RY = Value;
    }
}