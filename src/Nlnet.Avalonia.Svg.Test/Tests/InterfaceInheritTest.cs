using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlnet.Avalonia.Svg.Test.Tests
{
    public interface I
    {
        public string Name { get; }
    }

    public interface IA : I
    {
        string I.Name => "IA";
    }

    public interface IB : I
    {
        string I.Name => "IB";
    }

    public class Implementation1 : IA, IB
    {
        public string Name => "Implementation1";
    }

    public interface IG<T> where T : class, IG<T>
    {
        public string Name { get; }
    }

    public interface IC : IG<IC>
    {
        string IG<IC>.Name => "IC";
    }

    public interface ID : IG<ID>
    {
        string IG<ID>.Name => "ID";
    }

    public class Implementation2 : IC, ID
    {
        //public string Name => "Implementation2";
    }


    [TestClass]
    public class InterfaceInheritTest
    {
        [TestMethod]
        public void TestInherit()
        {
            var impl1 = new Implementation1();
            Assert.IsTrue(impl1.Name          == "Implementation1");
            Assert.IsFalse((impl1 as IA).Name == "IA");
            Assert.IsFalse((impl1 as IB).Name == "IB");
            Assert.IsTrue((impl1 as I).Name   == "Implementation1");

            var impl2 = new Implementation2();
            //Assert.IsTrue(impl2.Name         == "Implementation");
            Assert.IsTrue((impl2 as IC).Name     == "IC");
            Assert.IsTrue((impl2 as ID).Name     == "ID");
            Assert.IsTrue((impl2 as IG<IC>).Name == "IC");
            Assert.IsTrue((impl2 as IG<ID>).Name == "ID");
        }
    }
}
