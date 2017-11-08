using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalcLib.Maeda;
using CalcLib.Maeda.Finance;
using System.Collections.Generic;
using System.Linq;
using CalcLib;

namespace CalcLibTest.Maeda
{
    [TestClass]
    public class FinanceSvcTest
    {
        class PrevContext : ICalcContext
        {
            public string DisplayText { get; set; }

            public string SubDisplayText { get; set; }
        }

        FinanceContext ctx;
        FinanceSvc svc;

        [TestInitialize]
        public void Setup()
        {
            svc = new FinanceSvc();
            ctx = svc._CreateContext();
        }


        [TestMethod]
        public void TestFinanceMaeda_証券コード4桁のデータ引継ぎ()
        {
            svc.OnEnter(ctx, new CalcLib.Maeda.Dispatcher.SvcSwichedEventArg(new PrevContext
            {
                DisplayText = "1,301"
            }));

            // 株価ボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 1000 JPY", ctx.DisplayText);

            // 4 を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn4);

            // false が返される
            Assert.IsFalse(ret);
            Assert.AreEqual("", ctx.SubDisplayText);
        }
    }
}