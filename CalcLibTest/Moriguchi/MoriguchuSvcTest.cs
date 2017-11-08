using CalcLib;
using CalcLib.Moriguchi;
using CalcLib.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Test1()
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
    }
}
