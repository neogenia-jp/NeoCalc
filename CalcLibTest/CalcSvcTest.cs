using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalcLibTest
{
    [TestClass]
    public class CalcSvcTest
    {
        /// <summary>
        /// 足し算テスト
        /// </summary>
        [TestMethod]
        public void TestPlus()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1桁2口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("3", ctx.DisplayText);

            // 1桁3口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("6", ctx.DisplayText);

            // 2桁2口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("57", ctx.DisplayText);
        }

        /// <summary>
        /// 引き算テスト
        /// </summary>
        [TestMethod]
        public void TestMinus()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1桁2口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMinus);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("-1", ctx.DisplayText);

            // 1桁3口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn9);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMinus);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMinus);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("3", ctx.DisplayText);

            // 2桁2口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMinus);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("-33", ctx.DisplayText);
        }

        /// <summary>
        /// 掛け算テスト
        /// </summary>
        [TestMethod]
        public void TestMultiple()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1桁2口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("2", ctx.DisplayText);

            // 1桁3口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn9);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("135", ctx.DisplayText);

            // 2桁2口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("540", ctx.DisplayText);
        }

        /// <summary>
        /// 割り算テスト
        /// </summary>
        [TestMethod]
        public void TestDivide()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1桁2口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDivide);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("0.5", ctx.DisplayText);

            // 1桁3口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn9);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDivide);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDivide);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("1", ctx.DisplayText);

            // 2桁2口
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDivide);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("3", ctx.DisplayText);

            // 割り切れない
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDivide);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("0.333333333333333", ctx.DisplayText);
        }
    }
}