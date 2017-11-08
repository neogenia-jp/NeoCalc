using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalcLib.Maeda;
using CalcLib.Maeda.Finance;
using System.Collections.Generic;
using System.Linq;
using CalcLib;
using CalcLib.Util;

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
            public StockPrice GetStockPrice(string code) => new StockPrice(code, 2509m, new DateTime(2015, 5, 30));
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
            Assert.AreEqual("", ctx.SubDisplayText);
            Assert.AreEqual("[1301] 2509 JPY", ctx.DisplayText);

            // 4 を押す
            ret = svc.TryButtonClick(ctx, CalcLib.CalcButton.Btn4);

            // false が返される
            Assert.IsFalse(ret);
            Assert.AreEqual("", ctx.SubDisplayText);
        }
    }
}