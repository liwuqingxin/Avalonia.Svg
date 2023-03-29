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
    public interface IMarkerable : IMarkerStartSetter, IMarkerEndSetter, IMarkerMidSetter
    {
        public void RenderMarkerStart(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath);
        
        public void RenderMarkerEnd(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath);

        public void RenderMarkerMid(DrawingContext dc, ISvgContext ctx, SvgMarker marker, SKPath effectivePath);
    }
}
