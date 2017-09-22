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

        public void サブディスプレイのテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1 + 2.2 - 3333

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("1", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("1", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("2.2", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);

            Assert.AreEqual("1 + 2.2 ×", ctx.SubDisplayText);
            Assert.AreEqual("2.2", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMinus);

            Assert.AreEqual("1 + 2.2 -", ctx.SubDisplayText);
            Assert.AreEqual("2.2", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);

            Assert.AreEqual("1 + 2.2 -", ctx.SubDisplayText);
            Assert.AreEqual("3,333", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("-3,329.8", ctx.DisplayText);
        }

        public void BacnSpaceのテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1 + 2 + <BS> 44 <BS> 5 =

            // 1
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("1", ctx.DisplayText);

            // +
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("1", ctx.DisplayText);

            // 2
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("2", ctx.DisplayText);

            // + 
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("3", ctx.DisplayText);

            // <BS>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnBS);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("3", $"{ctx.DisplayText}");

            // 44
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("44", ctx.DisplayText);

            // <BS>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnBS);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("4", $"{ctx.DisplayText}");

            // 5
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("45", ctx.DisplayText);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("48", ctx.DisplayText);
        }

        
        public void ClearEndのテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1 + 2 + <CE> 44 <CE> 5 =

            // 1
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("1", ctx.DisplayText);

            // +
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("1", ctx.DisplayText);

            // 2
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("2", ctx.DisplayText);

            // + 
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("3", ctx.DisplayText);

            // <CE>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnClearEnd);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("0", $"{ctx.DisplayText}");

            // 44
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("44", ctx.DisplayText);

            // <CE>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnClearEnd);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("0", $"{ctx.DisplayText}");

            // 5
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("5", ctx.DisplayText);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("8", ctx.DisplayText);
        }

        
        public void Clearのテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1 + 2 + 4 <C> 5 =

            // 1
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("1", ctx.DisplayText);

            // +
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("1", ctx.DisplayText);

            // 2
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("2", ctx.DisplayText);

            // + 
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("3", ctx.DisplayText);

            // 4
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);

            Assert.AreEqual("1 + 2 + ", ctx.SubDisplayText);
            Assert.AreEqual("4", ctx.DisplayText);

            // <C>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnClear);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("0", $"{ctx.DisplayText}");

            // 5
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);

            Assert.AreEqual("", ctx.SubDisplayText);
            Assert.AreEqual("5", ctx.DisplayText);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("5", ctx.DisplayText);
        }
    }
}
