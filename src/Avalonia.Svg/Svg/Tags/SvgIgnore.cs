namespace Avalonia.Svg
{
    /// <summary>
    /// Represent a tag that is invalid or ineffective.
    /// </summary>
    [SvgTag(SvgTags.ignore)]
    public class SvgIgnore : SvgTagBase
    {
        private SvgIgnore()
        {
            
        }

        public static SvgIgnore Default { get; } = new SvgIgnore();
    }
}
