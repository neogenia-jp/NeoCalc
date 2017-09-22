using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalcLibTest
{
    [TestClass]
    public class CalcSvcTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("3", ctx.DisplayText);
        }
    }
}
