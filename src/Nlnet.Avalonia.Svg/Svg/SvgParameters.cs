namespace Nlnet.Avalonia.Svg
{
    public class SvgParameters
    {
        private const bool IfThrow = true;

        public static SvgParameters Default { get; set; } = new SvgParameters()
        {
#if DEBUG
            ThrowExceptionWhenTagNotRecognized = true,
            ThrowExceptionWhenTagNotImplement = true,
            ThrowExceptionWhenAttributeNotImplement = false,
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
