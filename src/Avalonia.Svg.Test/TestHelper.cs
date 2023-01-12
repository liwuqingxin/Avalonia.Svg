namespace Avalonia.Svg.Test
{
    internal static class TestHelper
    {
        public static void RunTestInAvaloniaAppEnvironment(Action action)
        {
            Assert.ThrowsException<AggregateException>(() =>
            {
                AvaloniaAppBuilder.BuildAvaloniaApp(action.Invoke);
            });
        }
    }
}
