using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Media;

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Some helper methods for svg.
    /// </summary>
    internal static class SvgHelper
    {
        /// <summary>
        /// Get transform for aligning all visuals to (0,0) on the whole.
        /// </summary>
        /// <param name="rects"></param>
        /// <returns></returns>
        public static Transform GetAlignToTopLeftTransform(IEnumerable<Rect> rects)
        {
            var topLeftPoints = rects.Select(r => r.TopLeft);
            // ReSharper disable once PossibleMultipleEnumeration
            var minX = topLeftPoints.Select(p => p.X).Min();
            // ReSharper disable once PossibleMultipleEnumeration
            var minY = topLeftPoints.Select(p => p.Y).Min();

            return new TranslateTransform(-minX, -minY);
        }

        /// <summary>
        /// Provide a generic visitor for <see cref="ISvgTag"/> tree.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="visitor"></param>
        public static void VisitSvgTagTree(this ISvgTag? tag, Action<ISvgTag> visitor)
        {
            if (tag == null)
            {
                return;
            }

            visitor(tag);

            if (tag.Children == null)
            {
                return;
            }

            foreach (var child in tag.Children)
            {
                child.VisitSvgTagTree(visitor);
            }
        }

        /// <summary>
        /// Provide a generic visitor for <see cref="ISvgTag"/> tree.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="visitor">if should go on visiting children of this tag, please return true.</param>
        private static void VisitSvgTagTree(this ISvgTag? tag, Func<ISvgTag, bool> visitor)
        {
            if (tag == null)
            {
                return;
            }

            var goon = visitor(tag);
            if (goon == false)
            {
                return;
            }

            if (tag.Children == null)
            {
                return;
            }

            foreach (var child in tag.Children)
            {
                child.VisitSvgTagTree(visitor);
            }
        }

        /// <summary>
        /// Render children.
        /// </summary>
        /// <param name="children"></param>
        /// <param name="dc"></param>
        public static void RenderRecursively(this List<ISvgTag>? children, DrawingContext dc)
        {
            children?.ForEach(c => c.VisitSvgTagTree(tag =>
            {
                if (tag is not ISvgRenderable renderable)
                {
                    return true;
                }

                renderable.Render(dc);
                return !renderable.RenderBySelf;
            }));
        }

        /// <summary>
        /// Try to parse a url definition and get the url.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool TryParseUrl(this string original, out string url, out string? token)
        {
            //
            // https://www.w3.org/TR/SVG2/painting.html#SpecifyingPaint
            //
            // url example : "url(#linearGradient-1)" or "url(#linearGradient-1) blue"
            //
            var match = Regex.Match(original, "url\\(\\#(.*)\\)\\s*(.*)");
            if (match.Success)
            {
                url = match.Groups[1].Value;
                token = match.Groups[2].Value;
                return true;
            }

            url    = string.Empty;
            token = null;
            return false;
        }

        /// <summary>
        /// Try to parse a href and get the id.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool TryParseHref(this string original, out string id)
        {
            //
            // https://www.w3.org/TR/SVG2/pservers.html#LinearGradientAttributes
            //
            // url example : "#linearGradient-1"
            //
            var match = Regex.Match(original, "\\#(.*)");
            if (match.Success)
            {
                id = match.Groups[1].Value;
                return true;
            }

            id = string.Empty;
            return false;
        }
    }
}
