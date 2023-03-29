namespace Nlnet.Avalonia.Svg;

public enum SvgMarkerOrientMode
{
    auto = 0,
    auto_start_reverse = 1,
    angle = 2,
}

public class SvgMarkerOrient
{
    public static SvgMarkerOrient Default { get; } = new();

    public SvgMarkerOrientMode Mode { get; set; } = SvgMarkerOrientMode.auto;

    public double Angle { get; set; }
}