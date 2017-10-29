using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    class StockSvc : ISubSvc
    {
        /// <summary>
        /// 株価取得モード用コンテキスト
        /// </summary>
        public class StockContext : ISubContext
        {
            /// <summary>
            /// メインディスプレイに表示する文字列
            /// </summary>
            public string DisplayText { get; }

            /// <summary>
            /// サブディスプレイに表示する文字列
            /// </summary>
            public string SubDisplayText { get; }

            //証券コード
            public string shokenCode;
        }

        public virtual ISubContext CreateContext() => new StockContext();
        
        /// <summary>
        /// 初期動作
        /// </summary>
        /// <param name="ctx0"></param>
        public void Init(ICalcContext ctx0)
        {
            var ctx = ctx0 as CalcSvcMoriguchi.ContextMoriguchi;
            //株価取得モードボタン押下時
            ctx.Value = "証券コードを入力して下さい";
            ctx.Operation = CalcButton.BtnExt2;
            ctx.Buffer ="";
        }

        /// <summary>
        /// クリック時動作
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public bool OnClick(ICalcContext ctx0, CalcButton btn)
        {
            string code = btn.ToString();


            return GetStock(ctx0, btn);
        }

        /// <summary>
        /// 証券コードから株価を取得する
        /// </summary>
        /// <param name="KabukaCode"></param>
        /// <returns></returns>
        public bool GetStock(ICalcContext ctx0, CalcButton btn)
        {
            var StockCtx = new StockContext();
            var ctx = (CalcSvcMoriguchi.ContextMoriguchi)ctx0;
            var code = (int)btn;

            if (code < 10)
            {
                StockCtx.shokenCode += code.ToString();
            }

            ctx.Buffer = "1000";

            return true;
        }
    }
}
