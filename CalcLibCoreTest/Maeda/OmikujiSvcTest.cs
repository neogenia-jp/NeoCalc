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

        TestOmikujiImple mock;
        OmikujiContext ctx;
        OmikujiSvc svc;

        [TestInitialize]
        public void Setup()
        {
            svc = new OmikujiSvc();
            ctx = svc._CreateContext();
            mock = new TestOmikujiImple();
            ctx.Proxy = mock;
        }

        public void SetKuji(params Kuji[] kuji) => (ctx.Proxy as TestOmikujiImple).Kuji = kuji.ToList();

        [TestMethod]
        public void TestOmikujiMaeda_選択したくじがSubDisplayTextに表示されること()
        {
            // おみくじの内部配置を設定
            mock.Kuji = new List<Kuji> {
                new Kuji(10, "大吉"),
                new Kuji(6, "中吉"),
                new Kuji(3, "小吉"),
                new Kuji(-5, "凶"),
            };
        
            // おみくじボタンを押す
            svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
            Assert.AreEqual("おみくじを選択して下さい", ctx.SubDisplayText);

            // 4 を選択
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn4);

            Assert.IsTrue(ret);
            Assert.AreEqual("大吉 中吉 小吉 凶　", ctx.DisplayText);
            Assert.AreEqual("本日の運勢は「凶　」です", ctx.SubDisplayText);

            // 1 を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn1);
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void TestOmikujiMaeda_ハードモードなくじ引き()
        {
            // おみくじの内部配置を設定
            mock.Kuji = new List<Kuji> {
                new Kuji(1, "末吉"),
                new Kuji(5, "吉"),
                new Kuji(0, "普通"),
                new Kuji(-10, "大凶"),
            };
        
            // おみくじボタンを押す
            svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.AreEqual("[1 ] [2 ] [3 ] [4 ]", ctx.DisplayText);
            Assert.AreEqual("おみくじを選択して下さい", ctx.SubDisplayText);

            // 1 を選択
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("末吉 吉　 普通 大凶", ctx.DisplayText);
            Assert.AreEqual("本日の運勢は「末吉」です", ctx.SubDisplayText);

            // 1 を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn1);
            Assert.IsFalse(ret);
        }
    }
}