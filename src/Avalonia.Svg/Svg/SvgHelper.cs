using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;

namespace Avalonia.Svg
{
    /// <summary>
    /// Some helper methods for svg.
    /// </summary>
    internal static class SvgHelper
    {
        /// <summary>
        /// Get normalized geometries from geometry data list.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, Geometry> GetGeometries(IReadOnlyDictionary<string, string> data)
        {
            var list = new Dictionary<string, Geometry>();
            foreach (var pair in data)
            {
                if (TryGetGeometry(pair.Value, out var geometry))
                {
                    list.Add(pair.Key, geometry!);
                }
            }

            list = NormalizeGeometries(list);

            return list;
        }

        private static bool TryGetGeometry(string data, out Geometry? geometry)
        {
            try
            {
                geometry = Geometry.Parse(data);
                return true;
            }
            catch
            {
                geometry = null;
                return false;
            }
        }

        /// <summary>
        /// Normalize the geometries.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static Dictionary<string, Geometry> NormalizeGeometries(IReadOnlyDictionary<string, Geometry> list)
        {
            var topLeftPoints = list.Select(pair => pair.Value.Bounds.TopLeft);
            // ReSharper disable once PossibleMultipleEnumeration
            var minX = topLeftPoints.Select(p => p.X).Min();
            // ReSharper disable once PossibleMultipleEnumeration
            var minY = topLeftPoints.Select(p => p.Y).Min();

            var resultList = list.Select(pair => (pair.Key, pair.Value.Clone())).ToDictionary(tuple => tuple.Key, tuple => tuple.Item2);

            foreach (var value in resultList.Values)
            {
                value.Transform = new TranslateTransform(-minX, -minY);
            }

            return resultList;
        }

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
    }
}
