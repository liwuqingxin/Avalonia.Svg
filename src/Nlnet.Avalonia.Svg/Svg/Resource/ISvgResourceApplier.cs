namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// A resource applier like class applier.
    /// </summary>
    public interface ISvgResourceApplier
    {
        /// <summary>
        /// Generally this apply one resource to the <see cref="tag"/> with service of <see cref="collector"/>.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="collector"></param>
        public void Apply(ISvgTag tag, ISvgResourceCollector collector);
    }
}
