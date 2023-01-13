namespace Avalonia.Svg
{
    public class SvgParameters
    {
        private const bool IfThrow = false;

        public static SvgParameters Default { get; set; } = new SvgParameters()
        {
#if DEBUG
            ThrowExceptionWhenTagNotRecognized      = IfThrow,
            ThrowExceptionWhenTagNotImplement       = IfThrow,
            ThrowExceptionWhenAttributeNotImplement = IfThrow,
#else
            ThrowExceptionWhenTagNotRecognized = false,
            ThrowExceptionWhenTagNotImplement = false,
            ThrowExceptionWhenAttributeNotImplement = false,
#endif
        };

        public bool ThrowExceptionWhenTagNotRecognized { get; set; }
        
        public bool ThrowExceptionWhenTagNotImplement { get; set; }

        public bool ThrowExceptionWhenAttributeNotImplement { get; set; }
    }
}
