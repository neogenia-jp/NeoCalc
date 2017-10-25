using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.RegularExpressions;
using CalcLib;

namespace CalcLibTest
{
    [TestClass]
    public class CalcSvcTestOmikuji
    {
        static readonly CalcButton omikujiBtn = CalcButton.BtnExt2;

        ICalcContext ctx;
        ICalcSvc svc;

        [TestInitialize]
        public void Setup()
        {
            svc = Factory.CreateService();
            ctx = svc.CreateContext();
        }

        private static void AssertOmikuji(ICalcContext ctx)
        {
            Assert.IsTrue(
                Enumerable.SequenceEqual(new[] { "大吉", "中吉", "小吉", "凶　" }.OrderBy(x => x), ctx.DisplayText.Split(' ').OrderBy(x => x)),
                $"DisplayTextの期待値が異なります: [{ctx.DisplayText}");
            Assert.IsTrue(
                Regex.Match(ctx.SubDisplayText, "本日の運勢は「(大吉|中吉|小吉|凶　)」です").Success,
                $"SubDisplayテキストの期待値が異なります: [{ctx.SubDisplayText}]");
        }

        [TestMethod]
        public void TestOmikuji_基本動作確認()
        {
            // おみくじボタン押下
            svc.OnButtonClick(ctx, omikujiBtn);

            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
            Assert.AreEqual("おみくじを選択して下さい", ctx.SubDisplayText);

            // 1キー押下
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            AssertOmikuji(ctx);

            // さらに2キー押下
            svc.OnButtonClick(ctx, CalcButton.Btn2);

            Assert.AreEqual("2", ctx.DisplayText);
            Assert.AreEqual("", ctx.SubDisplayText);
        }

        [TestMethod]
        public void TestOmikuji_１から４以外のキー押下時は何も起こらないこと()
        {
            // おみくじボタン押下
            svc.OnButtonClick(ctx, omikujiBtn);

            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
            Assert.AreEqual("おみくじを選択して下さい", ctx.SubDisplayText);

            foreach (var btn in Enum.GetValues(typeof(CalcButton)).Cast<CalcButton>().Except(new[]
            {
                CalcButton.Btn1,
                CalcButton.Btn2,
                CalcButton.Btn3,
                CalcButton.Btn4,
                CalcButton.BtnClear,
                CalcButton.BtnClearEnd,
                CalcButton.BtnExt1,
                CalcButton.BtnExt2,
                CalcButton.BtnExt3,
                CalcButton.BtnExt4,
            }))
            {
                // キー押下、何も起こらない。
                svc.OnButtonClick(ctx, btn);
                Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
                Assert.AreEqual("おみくじを選択して下さい", ctx.SubDisplayText);
            }

            // 4キー押下
            svc.OnButtonClick(ctx, CalcButton.Btn4);
            AssertOmikuji(ctx);
        }
        
        [TestMethod]
        public void TestOmikuji_ClearおよびClearEndおよびおみくじボタンで電卓モードに戻れること()
        {
            // おみくじボタン押下
            svc.OnButtonClick(ctx, omikujiBtn);

            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
            Assert.AreEqual("おみくじを選択して下さい", ctx.SubDisplayText);

            // Clearキー押下
            svc.OnButtonClick(ctx, CalcButton.BtnClear);
            Assert.AreEqual("0", ctx.DisplayText);
            Assert.AreEqual("", ctx.SubDisplayText);

            // おみくじボタン押下
            svc.OnButtonClick(ctx, omikujiBtn);

            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
            Assert.AreEqual("おみくじを選択して下さい", ctx.SubDisplayText);

            // ClearEndキー押下
            svc.OnButtonClick(ctx, CalcButton.BtnClearEnd);
            Assert.AreEqual("0", ctx.DisplayText);
            Assert.AreEqual("", ctx.SubDisplayText);

            // おみくじボタン押下
            svc.OnButtonClick(ctx, omikujiBtn);

            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
            Assert.AreEqual("おみくじを選択して下さい", ctx.SubDisplayText);

            // おみくじボタン押下
            svc.OnButtonClick(ctx, omikujiBtn);
            Assert.AreEqual("0", ctx.DisplayText);
            Assert.AreEqual("", ctx.SubDisplayText);
        }
    }
}
