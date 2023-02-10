using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg
{
    public class SvgContainer : SvgRenderable, ISvgContainer,
        IFillSetter,
        IStrokeSetter,
        IStrokeWidthSetter
    {
        public LightBrush? Fill
        {
            get;
            set;
        }
        public LightBrush? Stroke
        {
            get;
            set;
        }
        public double? StrokeWidth
        {
            get;
            set;
        }
    }
}
