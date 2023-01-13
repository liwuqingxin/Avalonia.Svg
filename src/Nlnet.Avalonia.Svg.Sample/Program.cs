using System;
using System.Diagnostics;
using System.IO;
using Avalonia;

namespace Nlnet.Avalonia.Svg.Sample
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // prepare and run your App here
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
            catch (Exception e)
            {
                // here we can work with the exception, for example add it to our log file
                // TODO Log.Fatal(e, "Something very bad happened");

                var location = $"{Path.GetDirectoryName(typeof(Program).Assembly.Location)}/Nlnet.Avalonia.MessageBox.exe";
                
                Process.Start(location, $"\"{e.Message}\" \"{e.StackTrace}\" \"发生不可恢复异常\"");

                if (Debugger.IsLogging())
                {
                    Debugger.Log(0, nameof(Program), e.ToString());
                }

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                else
                {
                    Debugger.Launch();
                }
            }
            finally
            {
                // This block is optional. 
                // Use the finally-block if you need to clean things up or similar
                //Log.CloseAndFlush();
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
        }
    }
}
