using CalcLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    class StockClass : ISubSvc
    {
        public class StockContext : ISubContext
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

        public virtual ISubContext CreateContext() => new StockContext();

        public void Init(ISubContext Factx, ICalcContext prevCtx)
        {
            GetStock(Factx, prevCtx);
        }

        /// <summary>
        /// 株価を取得する
        /// </summary>
        /// <param name="Factx"></param>
        /// <param name="prevCtx"></param>
        private void GetStock(ISubContext Factx, ICalcContext prevCtx)
        {
            var ctx = (StockContext)Factx;

            var code = prevCtx.DisplayText; // CalcSvcMoriguchi.ContextMoriguchi.Disp;

            //４桁だったら株価取得へ
            if (code.Length == 4)
            {
                try
                {
                    var sPrice = StockUraguchiUtil.GetStockPrice(code);

                    //株価取得成功時
                    ctx.DisplayText = $"[{code}] {sPrice.Price.ToString("#,0")} JPY";

                    //取引時間外で有れば「オワリネ」を表示
                    ctx.SubDisplayText = StockTimeUtil.isClosingTime(sPrice);
                    //ctx.SubDisplayText = sPrice.PriceGetDate.ToString();

                }

                catch (ApplicationException e)
                {
                    var ExText = e.Data["エラー種別"].ToString();
                    ctx.SubDisplayText = ExText;
                }
            }
            else
            {
                ctx.SubDisplayText = "INPUT ERROR";
            }
        }

        public bool OnClick(ISubContext Factx, CalcButton btn)
        {
            //Initで動作し、OnClickでモード終了する
            return false;
        }
    }
}
