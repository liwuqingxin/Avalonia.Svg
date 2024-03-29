﻿using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// Fills an area with a solid color. The difference between this and <see cref="ImmutableSolidColorBrush"/>
    /// is that some properties of this class can be modified.
    /// </summary>
    internal class LightSolidColorBrush : 
        LightBrush,
        ISolidColorBrush,
        IEquatable<LightSolidColorBrush>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Avalonia.Media.Immutable.ImmutableColorSolidColorBrush" /> class.
        /// </summary>
        /// <param name="color">The color to use.</param>
        /// <param name="opacity">The opacity of the brush.</param>
        /// <param name="transform">The transform of the brush.</param>
        public LightSolidColorBrush(Color color, double opacity = 1.0, ITransform? transform = null)
        {
            this.Color     = color;
            this.Opacity   = opacity;
            this.Transform = transform;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Avalonia.Media.Immutable.ImmutableColorSolidColorBrush" /> class.
        /// </summary>
        /// <param name="color">The color to use.</param>
        public LightSolidColorBrush(uint color)
          : this(Color.FromUInt32(color))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Avalonia.Media.Immutable.ImmutableColorSolidColorBrush" /> class.
        /// </summary>
        /// <param name="source">The brush from which this brush's properties should be copied.</param>
        public LightSolidColorBrush(ISolidColorBrush source)
        {
            var color = source.Color;
            var opacity = source.Opacity;
            var transform1 = source.Transform;
            var transform2 = transform1?.ToImmutable();

            this.Color     = color;
            this.Opacity   = opacity;
            this.Transform = transform2;
        }

        /// <summary>
        /// Gets the color of the brush.
        /// </summary>
        public Color Color { get; }
        
        public override LightBrush Clone()
        {
            return new LightSolidColorBrush(this);
        }

        public bool Equals(LightSolidColorBrush? other)
        {
            if ((object?)other == null)
                return false;
            if ((object)this == (object)other)
                return true;
            if (!this.Color.Equals(other.Color) || !this.Opacity.Equals(other.Opacity))
                return false;
            if (this.Transform == null && other.Transform == null)
                return true;
            return this.Transform != null && this.Transform.Equals((object?)other.Transform);
        }

        public override bool Equals(object? obj)
        {
            var other = obj as LightSolidColorBrush;
            return (object?)other != null && this.Equals(other);
        }

        public override int GetHashCode() => this.Color.GetHashCode() * 397 ^ this.Opacity.GetHashCode() ^ (this.Transform == null ? 0 : this.Transform.GetHashCode());

        public static bool operator ==(LightSolidColorBrush left, LightSolidColorBrush right) => object.Equals((object)left, (object)right);

        public static bool operator !=(LightSolidColorBrush left, LightSolidColorBrush right) => !object.Equals((object)left, (object)right);

        /// <summary>
        /// Returns a string representation of the brush.
        /// </summary>
        /// <returns>
        /// A string representation of the brush.
        /// </returns>
        public override string ToString() => this.Color.ToString();
    }
}
