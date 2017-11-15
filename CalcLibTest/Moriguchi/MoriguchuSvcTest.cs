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
            StockUtil2._Uraguchi(1000);

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
            
            StockUtil2._Uraguchi(1000);

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

            Assert.AreEqual("スクレイピングエラー", factx.SubDisplayText);
        }


        [TestMethod]
        public void Test4_ネットワーク通信エラーが発生した時()
        {
            var svc = new StockClass();
            var ctx = svc.CreateContext();

            var factx = new StockContext();

            StockUtil2._Uraguchi(1000);

            var webEx = new ApplicationException("エラーが発生しました", new WebException())
            {
                Data = { { "エラー種別", "ネットワークエラー" } }
            };
            StockUtil2._UraguchiExeption(webEx);

            //４桁以上の数字入力
            var prevCtx = new TestPecCtx
            {
                DisplayText = "1301"
            };
            svc.Init(factx, prevCtx);

            Assert.AreEqual("ネットワークエラー", factx.SubDisplayText);
        }


        [TestMethod]
        public void Test5_取引時間による株価取得時の時刻メッセージテスト()
        {
            var svc = new StockClass();
            var ctx = svc.CreateContext();

            var factx = new StockContext();

            //時間内の時（14時59分59秒取得）
            StockUtil2._UraguchiDate(new DateTime(2017,11,15,14,59,59));

            var prevCtx = new TestPecCtx
            {
                DisplayText = "1301"
            };
            svc.Init(factx, prevCtx);

            Assert.AreEqual("[1301] 1,000 JPY", factx.DisplayText);
            Assert.AreEqual("2017.11.15 14:59:59", factx.SubDisplayText);


            //時間外の時（15時00分00秒取得）
            StockUtil2._UraguchiDate(new DateTime(2017, 11, 15, 15, 0, 0));

            prevCtx = new TestPecCtx
            {
                DisplayText = "1301"
            };
            svc.Init(factx, prevCtx);

            Assert.AreEqual("[1301] 1,000 JPY", factx.DisplayText);
            Assert.AreEqual("2017.11.15オワリネ", factx.SubDisplayText);


            //日曜日に取得した場合()
            StockUtil2._UraguchiDate(new DateTime(2017, 11, 18, 15, 0, 0));

            prevCtx = new TestPecCtx
            {
                DisplayText = "1301"
            };
            svc.Init(factx, prevCtx);

            Assert.AreEqual("[1301] 1,000 JPY", factx.DisplayText);
            Assert.AreEqual("2017.11.17オワリネ", factx.SubDisplayText);


            svc.OnClick(factx, CalcLib.CalcButton.Btn0);
        }


    }
}
