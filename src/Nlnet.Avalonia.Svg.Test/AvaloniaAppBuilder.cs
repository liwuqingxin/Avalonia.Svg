using Avalonia;
using Avalonia.ReactiveUI;

namespace Nlnet.Avalonia.Svg.Test
{
    internal class AvaloniaAppBuilder : Application
    {
        public static Action? Action { get; set; }

        public static void BuildAvaloniaApp(Action? action)
        {
            Action = action;

            // prepare and run your App here
            BuildAvaloniaAppCore().StartWithClassicDesktopLifetime(new string[] { });
        }

        private static AppBuilder BuildAvaloniaAppCore()
        {
            return AppBuilder.Configure<AvaloniaApp>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        }
    }
}