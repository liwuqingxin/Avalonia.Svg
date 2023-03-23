namespace Nlnet.Avalonia.Svg
{
    /// <summary>
    /// A context applier like class applier.
    /// </summary>
    public interface ISvgContextApplier
    {
        /// <summary>
        /// Generally this apply one resource to the <see cref="tag"/> with service of <see cref="context"/>.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="context"></param>
        public void Apply(ISvgTag tag, ISvgContext context);
    }
}
