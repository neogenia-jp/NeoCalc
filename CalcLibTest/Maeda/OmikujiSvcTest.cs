using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalcLib.Maeda;
using CalcLib.Maeda.Omikuji;
using System.Collections.Generic;
using System.Linq;

namespace CalcLibTest.Maeda
{
    [TestClass]
    public class OmikujiSvcTest
    {
        class TestOmikujiImple : OmikujiBase
        {
            public List<Kuji> Kuji;

            protected override IEnumerable<Kuji> InitKuji() => Kuji;
        }

        OmikujiContext ctx;
        OmikujiSvc svc;

        [TestInitialize]
        public void Setup()
        {
            svc = new OmikujiSvc();
            ctx = svc._CreateContext();
            ctx.omikuji = new TestOmikujiImple();
        }

        public void SetKuji(params Kuji[] kuji) => (ctx.omikuji as TestOmikujiImple).Kuji = kuji.ToList();

        [TestMethod]
        public void TestKuji()
        {
            
            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
        }
    }
}