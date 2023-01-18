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
