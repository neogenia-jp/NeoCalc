using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalcLibTest
{
    [TestClass]
    public class CalcSvcTest2
    {
        [TestMethod]
        public void 小数の2進数演算誤差のテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 30000
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            // ×
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);

            // 0.0021
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);


            Assert.AreEqual("63", ctx.DisplayText);
        }
    }
}
