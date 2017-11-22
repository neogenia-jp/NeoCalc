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

            /// <summary>
            /// 証券コード
            /// </summary>
            public string code { get; set; }
        }

        public virtual ISubContext CreateContext() => new StockContext();

        public void Init(ISubContext Factx, ICalcContext prevCtx)
        {
            GetStock(Factx, prevCtx.DisplayText);
        }

        /// <summary>
        /// ボタン押下時の動作
        /// </summary>
        /// <param name="Factx"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public bool OnClick(ISubContext Factx, CalcButton btn)
        {            
            switch (btn)
            {
                //+キーで「日経平均株価」表示
                case CalcButton.BtnPlus:
                    break;

                //-キーで「ＮＹダウ平均」表示
                case CalcButton.BtnMinus:
                    break;

                //=キーで「株価再取得」
                case CalcButton.BtnEqual:
                    break;

                //それ以外で電卓モードで
                default:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 株価を取得する
        /// </summary>
        /// <param name="Factx"></param>
        /// <param name="inputCode"></param>
        private void GetStock(ISubContext Factx, string inputCode)
        {
            var ctx = (StockContext)Factx;

            ctx.code = inputCode;

            //４桁だったら株価取得へ
            if (ctx.code.Length == 4)
            {
                try
                {
                    var sPrice = StockUraguchiUtil.GetStockPrice(ctx.code);

                    //株価取得成功時
                    ctx.DisplayText = $"[{ctx.code}] {sPrice.Price.ToString("#,0")} JPY";

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
    }
}
