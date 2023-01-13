using System.Diagnostics;
using Nlnet.Avalonia.Svg.Sample;

namespace Nlnet.Avalonia.Svg.Test.Tests
{
    [TestClass]
    public class SvgTest
    {
        [TestMethod]
        public void TestNormal()
        {
            var type1 = typeof(double).ToString();
            var type2 = typeof(Double).ToString();

            Assert.IsTrue(type1 == type2);

            Console.Write("global::System.Collections.Generic.List<global::Avalonia.Point>");
        }

        [TestMethod]
        public void TestIconSvgHelper()
        {
            //
            // Test avalonia geometry algorithm and IconHelper class.
            //

            TestHelper.RunTestInAvaloniaAppEnvironment(() =>
            {
                var result = SvgHelper.GetGeometries(new Dictionary<string, string>()
                {
                    {
                        "1",
                        "M729 567.3c-17.4-12.2-28.8-32.5-28.8-55.4s11.4-43.1 28.8-55.4c17.9-12.5 22.7-36.9 13.2-56.5-5.4-11-11.5-21.6-18.3-31.7-12.2-18-35.7-25.8-55.4-16.6-19.3 9-42.5 8.7-62.4-2.7-19.5-11.3-31.2-30.7-33.4-51.5-2.3-22.1-20.1-39.2-42.2-40.8-6.1-0.4-12.2-0.7-18.4-0.7-6.2 0-12.3 0.2-18.4 0.7-22.1 1.6-39.9 18.7-42.2 40.8-1.9 17.5-10.5 33.9-24.8 45.5 27.3 28.4 51.8 58.8 73.7 90.9 3.8-0.6 7.7-1 11.7-1 43.7 0 79 35.4 79 79 0 20.1-7.6 38.4-19.9 52.4 14.9 35.5 27.2 72.2 36.8 109.7 19.4-10.5 41.8-10.5 60.5-1.8 19.7 9.2 43.2 1.4 55.4-16.6 6.8-10.1 13-20.7 18.3-31.7 9.5-19.8 4.6-44.1-13.2-56.6z"
                    },
                    {
                        "2",
                        "M736.2 557c-14.7-10.3-23.5-27.2-23.5-45.2s8.8-34.8 23.5-45.2c22.2-15.6 29.7-46.7 17.3-72.2-5.6-11.5-12.1-22.7-19.2-33.3-15.9-23.4-46.4-32.4-71-21-16.3 7.6-35.3 6.7-50.8-2.2-15.2-8.8-25.4-24.5-27.3-42-3-28.1-25.6-49.9-53.8-51.9-12.8-0.9-25.8-0.9-38.6 0-28.2 2-50.8 23.9-53.8 51.9-1.9 17.5-12.1 33.2-27.3 42-15.5 9-34.5 9.8-50.8 2.2-24.6-11.4-55.1-2.4-71 21-7.2 10.6-13.7 21.8-19.2 33.3-12.4 25.5-5 56.5 17.3 72.2 14.7 10.3 23.5 27.2 23.5 45.2 0 17.9-8.8 34.8-23.5 45.2-22.2 15.6-29.7 46.7-17.3 72.2 5.6 11.5 12.1 22.7 19.2 33.3 15.9 23.4 46.4 32.4 71 21 16.3-7.6 35.3-6.7 50.8 2.2 15.2 8.8 25.4 24.5 27.3 42 3 28.1 25.6 49.9 53.8 51.9 6.4 0.5 12.9 0.7 19.3 0.7s12.9-0.2 19.3-0.7c28.2-2 50.8-23.9 53.8-51.9 1.9-17.5 12-33.2 27.3-42 15.5-9 34.5-9.8 50.8-2.2 24.6 11.4 55.2 2.4 71-21 7.2-10.6 13.7-21.8 19.2-33.3 12.3-25.5 4.9-56.5-17.3-72.2z m-5.2 61.3c-5.1 10.4-10.9 20.6-17.4 30.2-9.1 13.4-26.2 18.6-39.8 12.3-23.7-11-51.3-9.8-73.9 3.3-22.1 12.8-36.9 35.6-39.6 61-1.7 16-14.6 28.5-30.7 29.6-11.6 0.8-23.5 0.8-35 0-16.1-1.2-29-13.6-30.7-29.6-2.7-25.4-17.5-48.2-39.6-61-12.3-7.1-26.2-10.7-40.1-10.7-11.5 0-23.1 2.5-33.8 7.5-13.6 6.3-30.7 1-39.8-12.3-6.5-9.6-12.4-19.8-17.5-30.2-7.1-14.6-3.2-32.1 9.2-40.8 21.4-15 34.1-39.5 34.1-65.6 0-26.1-12.8-50.6-34.1-65.6-12.3-8.7-16.3-26.2-9.2-40.8 5.1-10.4 10.9-20.6 17.5-30.2 9.1-13.4 26.2-18.6 39.8-12.3 23.7 11 51.3 9.8 73.9-3.3 22.1-12.8 36.9-35.6 39.6-61 1.7-16 14.6-28.5 30.7-29.6 11.6-0.8 23.5-0.8 35 0 16.1 1.2 29 13.6 30.7 29.6 2.7 25.4 17.5 48.2 39.6 61 22.6 13 50.2 14.3 73.9 3.3 13.6-6.3 30.7-1 39.8 12.3 6.5 9.6 12.4 19.8 17.4 30.2 7.1 14.6 3.2 32.1-9.2 40.8-21.4 15-34.1 39.5-34.1 65.6 0 26.1 12.8 50.6 34.1 65.6 12.3 8.5 16.3 26.1 9.2 40.7z"
                    },
                    {
                        "3",
                        "M512 420.3c-50.5 0-91.5 41.1-91.5 91.5s41.1 91.5 91.5 91.5c50.5 0 91.5-41.1 91.5-91.5s-41-91.5-91.5-91.5z m0 158.1c-36.7 0-66.5-29.8-66.5-66.5s29.8-66.5 66.5-66.5 66.5 29.8 66.5 66.5-29.8 66.5-66.5 66.5z"
                    },
                });

                Assert.IsTrue(result.Count == 3, "result.Count == 3");
                Assert.IsTrue(result.Values.Select(g => g.Bounds.TopLeft).Any(p => Math.Abs(p.X) < 0.001), "result.Values.Select(g => g.Bounds.TopLeft).Any(p => Math.Abs(p.X) < 0.001)");
                Assert.IsTrue(result.Values.Select(g => g.Bounds.TopLeft).Any(p => Math.Abs(p.Y) < 0.001), "result.Values.Select(g => g.Bounds.TopLeft).Any(p => Math.Abs(p.Y) < 0.001)");
                Assert.IsTrue(result.Values.Select(g => g.Bounds.TopLeft).All(p => p.X >= 0), "result.Values.Select(g => g.Bounds.TopLeft).All(p => p.X >= 0)");
                Assert.IsTrue(result.Values.Select(g => g.Bounds.TopLeft).Any(p => p.Y >= 0), "result.Values.Select(g => g.Bounds.TopLeft).Any(p => p.Y >= 0)");
            });
        }

        [TestMethod]
        public void TestSvgTagFactory()
        {
            //
            // Test svg tag factory functionality.
            //

            TestHelper.RunTestInAvaloniaAppEnvironment(() =>
            {
                foreach (var pair in SvgTagFactory.SvgTagFactories)
                {
                    Debug.WriteLine($"==={pair}");
                }

                foreach (var pair in SvgStyleSetterFactory.SvgSetterFactories)
                {
                    Debug.WriteLine($"--- {pair}");
                }

                var svg1 = SvgTagFactory.LoadSvg(TestData.SvgData1);
                var svg2 = SvgTagFactory.LoadSvg(TestData.SvgData2);
                var svg3 = SvgTagFactory.LoadSvg(TestData.SvgData3);
                var svg4 = SvgTagFactory.LoadSvg(TestData.SvgData4);

                Assert.IsTrue((svg1 as ISvgTag)!.GetTagName() == SvgTags.svg.ToString());
                Assert.IsTrue((svg2 as ISvgTag)!.GetTagName() == SvgTags.svg.ToString());
                Assert.IsTrue((svg3 as ISvgTag)!.GetTagName() == SvgTags.svg.ToString());
                Assert.IsTrue((svg4 as ISvgTag)!.GetTagName() == SvgTags.svg.ToString());
            });
        }
    }
}
