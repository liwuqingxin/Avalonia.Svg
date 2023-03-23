using Avalonia;

namespace Nlnet.Avalonia.Svg.Test
{
    public class AvaloniaApp : Application
    {
        public override void Initialize()
        {
            AvaloniaAppBuilder.Action?.Invoke();

            throw new AggregateException("Test in avalonia environment succeed.");
        }
    }
}