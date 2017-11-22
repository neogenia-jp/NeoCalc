using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalcLib.Maeda.Finance;
using System.Collections.Generic;
using System.Linq;
using CalcLib;
using CalcLib.Util;
using CalcLib.Maeda.Util;

namespace CalcLibTest.Maeda
{
    [TestClass]
    public class FinanceSvcTest
    {
        /// <summary>
        /// テスト用の切り替え元コンテキスト
        /// </summary>
        class PrevContext : ICalcContext
        {
            public string DisplayText { get; set; }

            public string SubDisplayText { get; set; }
        }

        /// <summary>
        /// テスト用のStockUtil
        /// </summary>
        class TestStockUtil : IStockUtil
        {
            public static decimal Counter = 0m;

            public StockPrice GetNikkei225() => new StockPrice("", 22000.15m + (Counter++), new DateTime(2016, 1, 10, 15, 0, 0));

            public StockPrice GetNyDow() => new StockPrice("", 19000.55m + (Counter++), new DateTime(2017, 3, 31, 10, 30, 0));

            public StockPrice GetStockPrice(string code) => new StockPrice(code, 2509m + (Counter++), new DateTime(2015, 5, 30, 9, 0, 0));
        }

        FinanceContext ctx;
        FinanceSvc svc;

        [TestInitialize]
        public void Setup()
        {
            svc = new FinanceSvc();
            ctx = svc._CreateContext();

            // テスト用のUtilクラスをActivatorに登録する
            CalcLib.Maeda.Finance.Imple.UtilActivatorImple.Registor<IStockUtil, TestStockUtil>();
            TestStockUtil.Counter = 0;

            TimeCop._CurrentTime = new DateTimeOffset(2000, 1, 1, 14, 59, 59, new TimeSpan(9, 0, 0));  // 現在日付を固定化。日本時間で 2000/01/01 14:59:59
        }

        [TestCleanup]
        public void TearDown()
        {
            TimeCop._CurrentTime = null;  // 現在日付の固定化を解除
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
            Assert.AreEqual("2015.05.30 09:00", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2,509 JPY", ctx.DisplayText);

            // 4 を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn4);

            // false が返される
            Assert.IsFalse(ret);
        }
        
        [TestMethod]
        public void TestFinanceMaeda_株価ボタンを押した場合はサービス終了()
        {
            svc.OnEnter(ctx, new CalcLib.Maeda.Dispatcher.SvcSwichedEventArg(new PrevContext
            {
                DisplayText = "1,301"
            }));

            // 株価ボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("2015.05.30 09:00", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2,509 JPY", ctx.DisplayText);

            // 再度株価ボタンを押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);

            // false が返される
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void TestFinanceMaeda_プラスボタン押下時は日経平均株価を取得できること()
        {
            svc.OnEnter(ctx, new CalcLib.Maeda.Dispatcher.SvcSwichedEventArg(new PrevContext
            {
                DisplayText = "1,301"
            }));

            // 株価ボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("2015.05.30 09:00", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2,509 JPY", ctx.DisplayText);

            // + を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            // true が返される
            Assert.IsTrue(ret);
            Assert.AreEqual("2016.01.10 15:00", ctx.SubDisplayText);
            Assert.AreEqual("[N225] 22,001.15 JPY", ctx.DisplayText);
        }

        [TestMethod]
        public void TestFinanceMaeda_マイナスボタン押下時はNYダウ平均を取得できること()
        {
            svc.OnEnter(ctx, new CalcLib.Maeda.Dispatcher.SvcSwichedEventArg(new PrevContext
            {
                DisplayText = "1,301"
            }));

            // 株価ボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("2015.05.30 09:00", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2,509 JPY", ctx.DisplayText);

            // - を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnMinus);

            // true が返される
            Assert.IsTrue(ret);
            Assert.AreEqual("2017.03.31 10:30", ctx.SubDisplayText);
            Assert.AreEqual("[DJI] 19,001.55 USD", ctx.DisplayText);
        }
        
        [TestMethod]
        public void TestFinanceMaeda_イコールボタン押下時は再取得()
        {
            svc.OnEnter(ctx, new CalcLib.Maeda.Dispatcher.SvcSwichedEventArg(new PrevContext
            {
                DisplayText = "1,301"
            }));

            // 株価ボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("2015.05.30 09:00", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2,509 JPY", ctx.DisplayText);

            // = を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            // 株価が再取得される
            Assert.IsTrue(ret);
            Assert.AreEqual("2015.05.30 09:00", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2,510 JPY", ctx.DisplayText);  // 1円上がってる！
            
            // - を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnMinus);

            // NYダウが取得される
            Assert.IsTrue(ret);
            Assert.AreEqual("2017.03.31 10:30", ctx.SubDisplayText);
            Assert.AreEqual("[DJI] 19,002.55 USD", ctx.DisplayText);

            // = を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            // NYダウが再取得される
            Assert.IsTrue(ret);
            Assert.AreEqual("2017.03.31 10:30", ctx.SubDisplayText);
            Assert.AreEqual("[DJI] 19,003.55 USD", ctx.DisplayText);  // 1ドル上がってる！

            // 0 を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn0);

            // false が返される
            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void TestFinanceMaeda_その他のボタン押下時はDisplayTextがクリアされること()
        {
            svc.OnEnter(ctx, new CalcLib.Maeda.Dispatcher.SvcSwichedEventArg(new PrevContext
            {
                DisplayText = "1,301"
            }));

            // 株価ボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("2015.05.30 09:00", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2,509 JPY", ctx.DisplayText);

            // * を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);

            // クリアされる
            Assert.IsTrue(ret);
            Assert.AreEqual("", ctx.SubDisplayText);
            Assert.AreEqual("", ctx.DisplayText);
    
            // = を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            // 株価が再取得される
            Assert.IsTrue(ret);
            Assert.AreEqual("2015.05.30 09:00", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2,510 JPY", ctx.DisplayText); 
        
            // / を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnDivide);

            // クリアされる
            Assert.IsTrue(ret);
            Assert.AreEqual("", ctx.SubDisplayText);
            Assert.AreEqual("", ctx.DisplayText);
        }

        [TestMethod]
        public void TestFinanceMaeda_オワリネの表示()
        {
            svc.OnEnter(ctx, new CalcLib.Maeda.Dispatcher.SvcSwichedEventArg(new PrevContext
            {
                DisplayText = "1,301"
            }));

            TimeCop._CurrentTime = new DateTimeOffset(2017, 11, 22, 14, 59, 59, new TimeSpan(9, 0, 0));  // 2017/11/22 14:59:59 JST

            // 株価ボタンを押す
            var ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnExt1);
            
            Assert.IsTrue(ret);
            Assert.AreEqual("2015.05.30 09:00", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2,509 JPY", ctx.DisplayText);

            TimeCop._CurrentTime = new DateTimeOffset(2017, 11, 22, 15, 0, 0, new TimeSpan(9, 0, 0));  // 2017/11/22 15:00:00 JST

            // = を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            // オワリネの表記になっているはず
            Assert.IsTrue(ret);
            Assert.AreEqual("2015.05.30 オワリネ", ctx.SubDisplayText);
            
            TimeCop._CurrentTime = new DateTimeOffset(2017, 11, 7, 6, 59, 59, new TimeSpan(9, 0, 0));  // 2017/11/6 16:59:59 JST

            // - を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnMinus);

            // NYダウが取得される
            Assert.IsTrue(ret);
            Assert.AreEqual("2017.03.31 10:30", ctx.SubDisplayText);

            TimeCop._CurrentTime = new DateTimeOffset(2017, 11, 7, 7, 0, 0, new TimeSpan(9, 0, 0));  // 2017/11/6 17:00:00 EST
            
            // = を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            // オワリネの表記になっているはず
            Assert.IsTrue(ret);
            Assert.AreEqual("2017.03.31 オワリネ", ctx.SubDisplayText);

            TimeCop._CurrentTime = new DateTimeOffset(2017, 11, 6, 6, 59, 59, new TimeSpan(9, 0, 0));  // 2017/11/5 15:59:59 EST (Summer Time)
            
            // = を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            // サマータイムがおわったので、まだオワリネではない
            Assert.IsTrue(ret);
            Assert.AreEqual("2017.03.31 10:30", ctx.SubDisplayText);

            TimeCop._CurrentTime = new DateTimeOffset(2017, 11, 6, 7, 0, 0, new TimeSpan(9, 0, 0));  // 2017/11/5 16:00:00 JST (Summer Time)
            
            // = を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            // オワリネの表記になっているはず
            Assert.IsTrue(ret);
            Assert.AreEqual("2017.03.31 オワリネ", ctx.SubDisplayText);
        }
    }
}