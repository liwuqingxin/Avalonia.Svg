using System.ComponentModel;
using System.Diagnostics;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
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

        /// <summary>
        /// Test the ability of <see cref="Color"/>.<see cref="Color.Parse(string)"/>.
        /// </summary>
        [TestMethod]
        public void TestParsingColor()
        {
            //h1 { color: maroon }
            //h2 { color: olive }

            // em { color: #f00 }              /* #rgb */
            // em { color: #ff0000 }           /* #rrggbb */
            // em { color: rgb(255,0,0) }
            // em { color: rgb(100%, 0%, 0%) }

            // em { color: rgb(255,0,0) }       /* integer range 0 - 255 */
            // em { color: rgb(300,0,0) }       /* clipped to rgb(255,0,0) */
            // em { color: rgb(255,-10,0) }     /* clipped to rgb(255,0,0) */
            // em { color: rgb(110%, 0%, 0%) }  /* clipped to rgb(100%,0%,0%) */

            // em { color: rgb(255,0,0) }       /* integer range 0 - 255 */
            // em { color: rgba(255,0,0,1)      /* the same, with explicit opacity of 1 */
            // em { color: rgb(100%,0%,0%) }    /* float range 0.0% - 100.0% */
            // em { color: rgba(100%,0%,0%,1) } /* the same, with explicit opacity of 1 */

            // * { color: hsl(0, 100%, 50%) }   /* red */
            // * { color: hsl(120, 100%, 50%) } /* lime */
            // * { color: hsl(120, 100%, 25%) } /* dark green */
            // * { color: hsl(120, 100%, 75%) } /* light green */
            // * { color: hsl(120, 75%, 75%) }  /* pastel green, and so on */

            // p { color: hsla(240, 100%, 50%, 0.5) } /* semi-transparent solid blue */
            // p { color: hsla(30, 100%, 50%, 0.1) }  /* very transparent solid orange */

            var c1  = Color.Parse("#f00");
            var c2  = Color.Parse("#3f00");
            var c3  = Color.Parse("#ff0000");
            var c4  = Color.Parse("#3399ff99");
            var c5  = Color.Parse("rgb(255,0,0)");
            var c6  = Color.Parse("rgba(255,0,0,1)");
            var c7  = Color.Parse("rgb(100%,0%,0%)");
            var c8  = Color.Parse("rgba(100%,0%,0%,1)");
            var c9  = Color.Parse("rgba(255,-10,300,1)");
            var c10 = Color.Parse("hsl(140, 100%, 50%)");
            var c11 = Color.Parse("hsla(160, 100%, 50%, 1)");
        }
    }
}
