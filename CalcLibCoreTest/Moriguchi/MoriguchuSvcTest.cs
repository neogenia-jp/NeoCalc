using CalcLib;
using CalcLib.Moriguchi;
using CalcLib.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static CalcLib.Moriguchi.StockClass;

namespace CalcLibTest.Moriguchi
{
    [TestClass]
    public class MoriguchuSvcTest
    {
        class TestPecCtx : ICalcContext
        {
            /// <summary>
            /// メインディスプレイに表示する文字列
            /// </summary>
            public string DisplayText { get; set; }

            /// <summary>
            /// サブディスプレイに表示する文字列
            /// </summary>
            public string SubDisplayText { get; set; }
        }

        [TestMethod]
        public void Test1_株価取得基本テスト()
        {
            var svc = new StockClass();
            var ctx = svc.CreateContext();

            var factx = new StockContext();

            // ここで、何とかして StockUtil が 1000 円を返すようにしたい！
            StockUraguchiUtil._Uraguchi(1000, new DateTime(2017, 11, 11, 11, 11, 11), DateTime.Now);

            var prevCtx = new TestPecCtx
            {
                DisplayText = "1301"
            };
            svc.Init(factx, prevCtx);

            Assert.AreEqual("[1301] 1,000 JPY", factx.DisplayText);

            svc.OnClick(factx, CalcLib.CalcButton.Btn0);
        }


        [TestMethod]
        public void Test2_4桁以外の数字が入力されている時()
        {
            var svc = new StockClass();
            var ctx = svc.CreateContext();

            var factx = new StockContext();

            StockUraguchiUtil._Uraguchi(1000, new DateTime(2017, 11, 11, 11, 11, 11), DateTime.Now);

            //４桁以上の数字入力
            var prevCtx = new TestPecCtx
            {
                DisplayText = "13012"
            };
            svc.Init(factx, prevCtx);

            Assert.AreEqual("INPUT ERROR", factx.SubDisplayText);

            //４桁未満の数字入力
            prevCtx = new TestPecCtx
            {
                DisplayText = "130"
            };
            svc.Init(factx, prevCtx);

            Assert.AreEqual("INPUT ERROR", factx.SubDisplayText);
        }


        [TestMethod]
        [Ignore]
        public void Test3_存在しない証券コードを入力した時()
        {
            var svc = new StockClass();
            var ctx = svc.CreateContext();

            var factx = new StockContext();

            var prevCtx = new TestPecCtx
            {
                DisplayText = "9999"
            };
            svc.Init(factx, prevCtx);

            Assert.AreEqual("SCRAPING ERROR", factx.SubDisplayText);
        }


        [TestMethod]
        public void Test4_ネットワーク通信エラーが発生した時()
        {
            var svc = new StockClass();
            var ctx = svc.CreateContext();

            var factx = new StockContext();

            StockUraguchiUtil._Uraguchi(1000, new DateTime(2017, 11, 11, 11, 11, 11), DateTime.Now);

            var webEx = new ApplicationException("エラーが発生しました", new WebException())
            {
                Data = { { "エラー種別", "ネットワークエラー" } }
            };
            StockUraguchiUtil._UraguchiExeption(webEx);

            //４桁以上の数字入力
            var prevCtx = new TestPecCtx
            {
                DisplayText = "1301"
            };
            svc.Init(factx, prevCtx);

            Assert.AreEqual("ネットワークエラー", factx.SubDisplayText);
        }


    //    [TestMethod]
    //    public void Test5_取引時間による株価取得時の時刻メッセージテスト()
    //    {
    //        var svc = new StockClass();
    //        var ctx = svc.CreateContext();

    //        var factx = new StockContext();

    //        //15:00前に更新された株価の取得
    //        StockUraguchiUtil._Uraguchi(1000, new DateTime(2017, 11, 15, 14, 59, 59), new DateTime(2017, 11, 15, 11, 15, 0));

    //        var prevCtx = new TestPecCtx
    //        {
    //            DisplayText = "1301"
    //        };
    //        svc.Init(factx, prevCtx);

    //        Assert.AreEqual("[1301] 1,000 JPY", factx.DisplayText);
    //        Assert.AreEqual("2017.11.15 14:55:00", factx.SubDisplayText);


    //        //15:00後に更新された株価の取得
    //        StockUraguchiUtil._Uraguchi(1000, new DateTime(2017, 11, 15, 15, 0, 0), new DateTime(2017, 11, 15, 17, 0, 0));

    //        prevCtx = new TestPecCtx
    //        {
    //            DisplayText = "1301"
    //        };
    //        svc.Init(factx, prevCtx);

    //        Assert.AreEqual("[1301] 1,000 JPY", factx.DisplayText);
    //        Assert.AreEqual("2017.11.15 オワリネ", factx.SubDisplayText);
            
    //        svc.OnClick(factx, CalcLib.CalcButton.Btn0);
    //    }



    //    //TODO:以下、実施予定のテスト


    //    [TestMethod]
    //    public void Test6_ＮＹダウ平均の取得テスト()
    //    {

    //    }

    //    [TestMethod]
    //    public void Test7_日経平均株価の取得テスト()
    //    {

    //    }



    }
}
