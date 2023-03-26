﻿using System;
using System.Collections.Generic;
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
        /// Provide a generic visitor for <see cref="ISvgTag"/> tree which does not include the root node.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="visitor"></param>
        public static void VisitSvgTagTreeWithoutSelf(this ISvgTag? tag, Action<ISvgTag> visitor)
        {
            if (tag?.Children == null)
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

            url = string.Empty;
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

        /// <summary>
        /// Get the scale factors for fill mode.
        /// </summary>
        /// <param name="parentSize"></param>
        /// <param name="childSize"></param>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        internal static void GetFillFactors(Size parentSize, Size childSize, out double scaleX, out double scaleY)
        {
            scaleX = parentSize.Width  / childSize.Width;
            scaleY = parentSize.Height / childSize.Height;
        }

        /// <summary>
        /// Get the scale factors for uniform stretch mode.
        /// </summary>
        /// <param name="parentSize"></param>
        /// <param name="childSize"></param>
        /// <param name="fill"></param>
        /// <param name="scale"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        internal static void GetUniformFactors(Size parentSize, Size childSize, bool fill, out double scale, out double offsetX, out double offsetY)
        {
            var scaleX = parentSize.Width  / childSize.Width;
            var scaleY = parentSize.Height / childSize.Height;
            scale   = fill ? Math.Max(scaleX, scaleY) : Math.Min(scaleX, scaleY);
            offsetX = (parentSize.Width  - childSize.Width  * scale) / 2;
            offsetY = (parentSize.Height - childSize.Height * scale) / 2;
        }
    }
}
