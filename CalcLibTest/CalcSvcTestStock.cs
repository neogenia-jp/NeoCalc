using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.RegularExpressions;
using CalcLib;

namespace CalcLibTest
{
    [TestClass]
    public class CalcSvcTestStock
    {
        static readonly CalcButton stockBtn = CalcButton.BtnExt3;

        ICalcContext ctx;
        ICalcSvc svc;

        [TestInitialize]
        public void Setup()
        {
            svc = Factory.CreateService();
            ctx = svc.CreateContext();
        }

        [TestMethod]
        public void TestStock_基本動作確認()
        {
            // 1,301
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcButton.Btn1);

            Assert.AreEqual("1,301", ctx.DisplayText);

            // 株価取得ボタン押下
            svc.OnButtonClick(ctx, stockBtn);

            Assert.AreEqual("[1301] 1,000 JPY", ctx.DisplayText);

            // 電卓にもどる
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            Assert.AreEqual("1", ctx.DisplayText);
        }

        [TestMethod]
        public void TestStock_おみくじモードから株価取得()
        {
            // おみくじモードへ
            svc.OnButtonClick(ctx, CalcButton.BtnExt2);
            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
            Assert.AreEqual("おみくじを選択して下さい", ctx.SubDisplayText);

            // 株価取得ボタン押下
            svc.OnButtonClick(ctx, stockBtn);

            // 証券コードが入力されていないため入力エラー
            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
            Assert.AreEqual("INPUT ERROR", ctx.SubDisplayText);

            // 電卓にもどる
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            Assert.AreEqual("1", ctx.DisplayText);
        }
    }
}
