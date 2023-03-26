using System;
using Avalonia;
using Avalonia.Media;
using System.Collections.Generic;
using System.Linq;

namespace Nlnet.Avalonia.Svg
{
    public class Svg : ISvg, IInitializable, ISvgContext
    {
        private readonly SvgSvg _svgTag;

        public Svg(SvgSvg svgTag)
        {
            _svgTag = svgTag;
        }



        #region ISvgContext

        private readonly List<ISvgStyle>                _styles      = new();
        private readonly Dictionary<string, LightBrush> _brushes     = new();
        private readonly Dictionary<string, ISvgTag>    _idTags      = new();
        private readonly List<ISvgRenderable>           _renderables = new();

        public bool ShowDiagnosis { get; private set; }

        public Size ContainerSize { get; private set; }
        
        IReadOnlyList<ISvgStyle> ISvgContext.Styles => this._styles;

        IReadOnlyDictionary<string, LightBrush> ISvgContext.Brushes => this._brushes;

        IReadOnlyDictionary<string, ISvgTag> ISvgContext.IdTags => this._idTags;

        IReadOnlyList<ISvgRenderable> ISvgContext.Renderables => this._renderables;

        private void PrepareContext()
        {
            // Prepare all elements with id.
            _svgTag.VisitSvgTagTree(tag =>
            {
                if (string.IsNullOrEmpty(tag.Id) == false)
                {
                    // BUG If id is duplicate, now we drop it.
                    this._idTags.TryAdd(tag.Id, tag);
                }
            });

            // Prepare styles, brushes, renderables...
            _svgTag.VisitSvgTagTree(tag =>
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (tag is ISvgStyleProvider styleProvider)
                {
                    foreach (var style in styleProvider.GetStyles())
                    {
                        this._styles.Add(style);
                    }
                }

                if (tag is ISvgBrushProvider brushProvider)
                {
                    if (brushProvider.Id != null)
                    {
                        this._brushes.Add(brushProvider.Id, brushProvider.GetBrush(this));
                    }
                }

                if (tag is ISvgRenderable renderable && tag.IsDef == false)
                {
                    this._renderables.Add(renderable);
                }
            });
        }

        private void BuildContext()
        {
            foreach (var setter in _styles.SelectMany(style => style.Setters))
            {
                setter.ApplyDeferredValueString(this);
            }
        }

        #endregion



        #region IInitializable

        public void Initialize()
        {
            // Collect, build and apply svg context, and apply transforms.
            this.PrepareContext();
            this.BuildContext();

            _svgTag.ApplyContext(this);
        }

        #endregion



        #region ISvg

        void ISvg.Render(DrawingContext dc, Size availableSize, Stretch stretch, bool showDiagnosis)
        {
            this.ContainerSize = availableSize;
            this.ShowDiagnosis = showDiagnosis;

            var width  = _svgTag.Width  ?? _svgTag.ViewBox?.Width  ?? 0d;
            var height = _svgTag.Height ?? _svgTag.ViewBox?.Height ?? 0d;

            if (width != 0 && height != 0 && stretch != Stretch.None)
            {
                var childSize = new Size(width, height);
                var scaleX    = 0d;
                var scaleY    = 0d;
                var offsetX   = 0d;
                var offsetY   = 0d;

                switch (stretch)
                {
                    case Stretch.Fill:
                        SvgHelper.GetFillFactors(availableSize, new Size(width, height), out scaleX, out scaleY);
                        break;
                    case Stretch.Uniform:
                    {
                        SvgHelper.GetUniformFactors(availableSize, new Size(width, height), false, out var scale, out offsetX, out offsetY);
                        scaleX = scale;
                        scaleY = scale;
                        break;
                    }
                    case Stretch.UniformToFill:
                    {
                        SvgHelper.GetUniformFactors(availableSize, new Size(width, height), true, out var scale, out offsetX, out offsetY);
                        scaleX = scale;
                        scaleY = scale;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(stretch), stretch, null);
                }

                using (dc.PushPostTransform(Matrix.CreateScale(scaleX, scaleY)))
                using (dc.PushPostTransform(Matrix.CreateTranslation(offsetX, offsetY)))
                using (dc.PushTransformContainer())
                    _svgTag.Render(dc, this);
            }
            else
            {
                _svgTag.Render(dc, this);
            }
        }

        Size ISvg.GetDesiredSize(Size availableSize)
        {
            var width  = _svgTag.Width  ?? availableSize.Width;
            var height = _svgTag.Height ?? availableSize.Height;

            if (double.IsNaN(width) || double.IsInfinity(width))
            {
                return Size.Empty;
            }
            if (double.IsNaN(height) || double.IsInfinity(height))
            {
                return Size.Empty;
            }

            return new Size(width, height);
        }

        #endregion
    }
}
