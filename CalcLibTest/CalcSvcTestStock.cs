using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.RegularExpressions;
using CalcLib;
using CalcLib.Util;

namespace CalcLibTest
{
    [TestClass]
    public class CalcSvcTestStock
    {
        static readonly CalcButton stockBtn = CalcButton.BtnExt3;

        ICalcContext ctx;
        ICalcSvc svc;
        StockUtilYamamotoWrapper wrapper;

        [TestInitialize]
        public void Setup()
        {
            svc = Factory.CreateService();
            ctx = svc.CreateContext();
            wrapper = StockUtilYamamotoWrapper.GetInstance();
        }

        [TestMethod]
        public void TestStock_電卓と株価取得モードの切り替わり()
        {
            // 1301を入力
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcButton.Btn1);

            Assert.AreEqual("1,301", ctx.DisplayText);

            // 取得する株価を設定し、株価取得ボタン押下
            wrapper.RegistStockData(new StockPrice("1301", 1200m, new DateTime(2017, 11, 13, 12, 00, 00)));
            svc.OnButtonClick(ctx, stockBtn);

            Assert.AreEqual("[1301] 1,200 JPY", ctx.DisplayText);
            Assert.AreEqual("2017.11.13 12:00", ctx.SubDisplayText);

            // 電卓にもどる
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.BtnPlus);
            svc.OnButtonClick(ctx, CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcButton.BtnEqual);
            Assert.AreEqual("3", ctx.DisplayText);
            Assert.AreEqual("", ctx.SubDisplayText);
        }

        [TestMethod]
        public void TestStock_入力が4桁でない場合の表示()
        {
            // 3桁で入力
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcButton.Btn0);

            Assert.AreEqual("130", ctx.DisplayText);
            svc.OnButtonClick(ctx, stockBtn);

            Assert.AreEqual("", ctx.DisplayText);
            Assert.AreEqual("INPUT ERROR", ctx.SubDisplayText);

            // 電卓にもどる
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.BtnPlus);
            svc.OnButtonClick(ctx, CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcButton.BtnEqual);
            Assert.AreEqual("3", ctx.DisplayText);
            Assert.AreEqual("", ctx.SubDisplayText);
        }

        [TestMethod]
        public void TestStock_スクレイピングエラーの場合の表示()
        {
            // このテストではScrapingExceptionが返ってくることを想定する
            wrapper.RegistException(new StockUtilYamamoto.ScrapingException());

            // 存在しない銘柄(1000)を入力
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcButton.Btn0);

            Assert.AreEqual("1,000", ctx.DisplayText);

            // 株価取得ボタン押下
            svc.OnButtonClick(ctx, stockBtn);

            Assert.AreEqual("", ctx.DisplayText);
            Assert.AreEqual("SCRAPING ERROR", ctx.SubDisplayText);

            // 電卓にもどる
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.BtnPlus);
            svc.OnButtonClick(ctx, CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcButton.BtnEqual);
            Assert.AreEqual("3", ctx.DisplayText);
            Assert.AreEqual("", ctx.SubDisplayText);
        }

        [TestMethod]
        public void TestStock_Webアクセスに失敗したときの表示()
        {
            // このテストではWebExceptionが返ってくることを想定している
            wrapper.RegistException(new System.Net.WebException());

            // 銘柄(1301)を入力
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcButton.Btn1);

            Assert.AreEqual("1,301", ctx.DisplayText);

            // 取得する株価を設定し、株価取得ボタン押下
            svc.OnButtonClick(ctx, stockBtn);

            Assert.AreEqual("", ctx.DisplayText);
            Assert.AreEqual("SCRAPING ERROR", ctx.SubDisplayText);

            // 電卓にもどる
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.BtnPlus);
            svc.OnButtonClick(ctx, CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcButton.BtnEqual);
            Assert.AreEqual("3", ctx.DisplayText);
            Assert.AreEqual("", ctx.SubDisplayText);
        }

        [TestMethod]
        public void TestStock_日経平均株価の表示()
        {
            // 1301を入力
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            Assert.AreEqual("1,301", ctx.DisplayText);

            // 取得する株価を設定し、株価取得ボタン押下
            wrapper.RegistStockData(new StockPrice("1301", 1200m, new DateTime(2017, 11, 13, 12, 00, 00)));
            svc.OnButtonClick(ctx, stockBtn);
            Assert.AreEqual("[1301] 1,200 JPY", ctx.DisplayText);

            // 日経平均株価を設定し、日経平均株価を取得する
            wrapper.RegistStockData(new StockPrice(StockUtilYamamoto.N225_CODE, 21355.32m, new DateTime(2017, 11, 13, 12, 00, 01)));
            svc.OnButtonClick(ctx, CalcButton.BtnPlus);
            Assert.AreEqual("[N225] 21,355.32 JPY", ctx.DisplayText);

            // 1キー押下
            // 電卓にもどる
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            Assert.AreEqual("1", ctx.DisplayText);
        }

        [TestMethod]
        public void TestStock_NYダウの表示()
        {
            // 1301を入力
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            Assert.AreEqual("1,301", ctx.DisplayText);

            // 取得する株価を設定し、株価取得ボタン押下
            wrapper.RegistStockData(new StockPrice("1301", 1200m, new DateTime(2017, 11, 13, 12, 00, 00)));
            svc.OnButtonClick(ctx, stockBtn);
            Assert.AreEqual("[1301] 1,200 JPY", ctx.DisplayText);

            // NYダウを設定し、日経平均株価を取得する
            wrapper.RegistStockData(new StockPrice(StockUtilYamamoto.NY_DOW_CODE, 22997.44m, new DateTime(2017, 11, 13, 12, 00, 02)));
            svc.OnButtonClick(ctx, CalcButton.BtnMinus);
            Assert.AreEqual("[DJI] 22,997.44 USD", ctx.DisplayText);

            // 1キー押下
            // 電卓にもどる
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            Assert.AreEqual("1", ctx.DisplayText);
        }

        [TestMethod]
        public void TestStock_株価の再取得()
        {
            // 1301を入力
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            Assert.AreEqual("1,301", ctx.DisplayText);

            // 取得する株価を設定し、株価取得ボタン押下
            wrapper.RegistStockData(new StockPrice("1301", 1200m, new DateTime(2017, 11, 13, 12, 00, 00)));
            svc.OnButtonClick(ctx, stockBtn);
            Assert.AreEqual("[1301] 1,200 JPY", ctx.DisplayText);

            // 取得する株価を再設定し、株価取得ボタン押下
            wrapper.RegistStockData(new StockPrice("1301", 1201m, new DateTime(2017, 11, 13, 12, 00, 01)));
            svc.OnButtonClick(ctx, CalcButton.BtnEqual);
            Assert.AreEqual("[1301] 1,201 JPY", ctx.DisplayText);

            // 1キー押下
            // 電卓にもどる
            svc.OnButtonClick(ctx, CalcButton.Btn1);
            Assert.AreEqual("1", ctx.DisplayText);
        }
    }
}
