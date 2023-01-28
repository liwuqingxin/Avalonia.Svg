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
        public IBrush? Fill
        {
            get;
            set;
        }
        public IBrush? Stroke
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
