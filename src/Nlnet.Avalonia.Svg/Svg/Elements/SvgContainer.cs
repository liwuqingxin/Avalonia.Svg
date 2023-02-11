using Avalonia.Media;

namespace Nlnet.Avalonia.Svg
{
    public class SvgContainer : SvgRenderable, ISvgContainer,
        IFillSetter,
        IFillRuleSetter,
        IFillOpacitySetter,
        IStrokeSetter,
        IStrokeOpacitySetter,
        IStrokeWidthSetter,
        IStrokeLineCapSetter,
        IStrokeLineJoinSetter,
        IStrokeMiterLimitSetter,
        IStrokeDashArraySetter,
        IStrokeDashOffsetSetter
    {
        public LightBrush? Fill
        {
            get;
            set;
        }
        public FillRule? FillRule
        {
            get;
            set;
        }
        public double? FillOpacity
        {
            get;
            set;
        }
        public LightBrush? Stroke
        {
            get;
            set;
        }
        public double? StrokeOpacity
        {
            get;
            set;
        }
        public double? StrokeWidth
        {
            get;
            set;
        }
        public PenLineCap? StrokeLineCap
        {
            get;
            set;
        }
        public PenLineJoin? StrokeLineJoin
        {
            get;
            set;
        }
        public double? StrokeMiterLimit
        {
            get;
            set;
        }
        public DoubleList? StrokeDashArray
        {
            get;
            set;
        }
        public double? StrokeDashOffset
        {
            get;
            set;
        }
    }
}
