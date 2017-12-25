using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalcLib.Maeda;
using CalcLib.Maeda.YamamotoJanken;
using System.Collections.Generic;
using System.Linq;

namespace CalcLibTest.Maeda
{
    [TestClass]
    public class YamamotoJankenSvcTest
    {
        class TestYamamotoJankenHand : YamamotoJankenHandBase
        {
            public override void RandomSet()
            {
                return;
            }
        }

        private bool ClickGu() => svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn1);
        private bool ClickChoki() => svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn2);
        private bool ClickPa() => svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn3);

        YamamotoJankenContext ctx;
        YamamotoJankenSvc svc;

        [TestInitialize]
        public void Setup()
        {
            svc = new YamamotoJankenSvc();
            ctx = svc._CreateContext();
            ctx.NPC = new TestYamamotoJankenHand();
        }

        [TestMethod]
        public void TestYamamotoJankenSvc_ジャンケンに勝った場合()
        {
            // じゃんけんボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs ？？？", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // パーを選択
            ret = ClickPa();

            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs パー", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // = を押す(相手が返してくる手 -> グー)
            ctx.NPC.Hand = YamamotoJankenHandBase.JankenHands.Gu;
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.IsTrue(ret);
            Assert.AreEqual("グー vs パー", ctx.DisplayText);
            Assert.AreEqual("アナタノカチ！", ctx.SubDisplayText);
        }

        [TestMethod]
        public void TestYamamotoJankenSvc_ジャンケンに引き分けた場合()
        {
            // じゃんけんボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs ？？？", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // チョキを選択
            ret = ClickChoki();

            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs チョキ", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // = を押す(相手が返してくる手 -> チョキ)
            ctx.NPC.Hand = YamamotoJankenHandBase.JankenHands.Choki;
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs ？？？", ctx.DisplayText);
            Assert.AreEqual("アイコデショ...", ctx.SubDisplayText);

        }

        [TestMethod]
        public void TestYamamotoJankenSvc_ジャンケンに負けた場合()
        {
            // じゃんけんボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs ？？？", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // パーを選択
            ret = ClickPa();

            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs パー", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // = を押す(相手が返してくる手 -> チョキ)
            ctx.NPC.Hand = YamamotoJankenHandBase.JankenHands.Choki;
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.IsTrue(ret);
            Assert.AreEqual("チョキ vs パー", ctx.DisplayText);
            Assert.AreEqual("アナタノマケ！", ctx.SubDisplayText);

        }

        [TestMethod]
        public void TestYamamotoJankenSvc_ジャンケン中にCを押した場合は電卓モードに戻る()
        {
            // じゃんけんボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs ？？？", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // C を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnClear);
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void TestYamamotoJankenSvc_ジャンケン中にCEを押した場合は電卓モードに戻る()
        {
            // じゃんけんボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs ？？？", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // CE を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnClearEnd);
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void TestYamamotoJankenSvc_ジャンケン中にじゃんけんを押した場合は電卓モードに戻る()
        {
            // じゃんけんボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs ？？？", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // じゃんけんボタンを押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void TestYamamotoJankenSvc_ジャンケンに引き分けた後に連続で勝負できること()
        {
            // じゃんけんボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs ？？？", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // チョキを選択
            ret = ClickChoki();

            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs チョキ", ctx.DisplayText);
            Assert.AreEqual("ジャンケン...", ctx.SubDisplayText);

            // = を押す(相手が返してくる手 -> チョキ)
            ctx.NPC.Hand = YamamotoJankenHandBase.JankenHands.Choki;
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs ？？？", ctx.DisplayText);
            Assert.AreEqual("アイコデショ...", ctx.SubDisplayText);

            // グーを選択
            ret = ClickGu();

            Assert.IsTrue(ret);
            Assert.AreEqual("？？？ vs グー", ctx.DisplayText);
            Assert.AreEqual("アイコデショ...", ctx.SubDisplayText);

            // = を押す(相手が返してくる手 -> チョキ)
            ctx.NPC.Hand = YamamotoJankenHandBase.JankenHands.Choki;
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.IsTrue(ret);
            Assert.AreEqual("チョキ vs グー", ctx.DisplayText);
            Assert.AreEqual("アナタノカチ！", ctx.SubDisplayText);

            // じゃんけん終了後にキーを押すと電卓モードに戻る
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn1);
            Assert.IsFalse(ret);
        }

    }
}