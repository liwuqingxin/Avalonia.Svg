using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Means marker is available for this element.
    /// </summary>
    public interface ISvgMarkerable : IMarkerStartSetter, IMarkerEndSetter, IMarkerMidSetter, IMarkerSetter
    {
        public void RenderMarkerStart(DrawingContext dc, ISvgContext ctx, SvgMarker marker);
        
        public void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker);

        public void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker);
    }
}
