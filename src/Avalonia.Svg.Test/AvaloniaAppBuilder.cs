using System.Net.Mime;
using Avalonia.ReactiveUI;

namespace Avalonia.Svg.Test
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
            AvaloniaLocator.Current.GetService<AvaloniaNativePlatformOptions>();

            return AppBuilder.Configure<AvaloniaApp>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        }
    }
}