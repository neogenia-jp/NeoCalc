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
            public string Code { get; set; }
        }

        public virtual ISubContext CreateContext() => new StockContext();

        public void Init(ISubContext factx, ICalcContext prevCtx)
        {
            GetStock(factx, prevCtx.DisplayText);
        }

        /// <summary>
        /// ボタン押下時の動作
        /// </summary>
        /// <param name="Factx"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public bool OnClick(ISubContext factx, CalcButton btn)
        {
            var ctx = factx as StockContext;

            switch (btn)
            {
                //+キーで「日経平均株価」表示
                case CalcButton.BtnPlus:

                    break;

                //-キーで「ＮＹダウ平均」表示
                case CalcButton.BtnMinus:
                    GetDow(factx);
                    break;

                //=キーで「株価再取得」
                case CalcButton.BtnEqual:
                    GetStock(factx, ctx.Code);
                    break;

                //[株価取得]押下時は何もしない
                case CalcButton.BtnExt3:
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

            ctx.Code = inputCode;

            //４桁だったら株価取得へ
            if (ctx.Code.Length == 4)
            {
                try
                {
                    var sPrice = StockUraguchiUtil.GetStockPrice(ctx.Code);

                    //株価取得成功時
                    ctx.DisplayText = $"[{ctx.Code}] {sPrice.Price.ToString("#,0")} JPY";

                    //取引時間外で有れば「オワリネ」を表示
                    ctx.SubDisplayText = StockTimeUtil.CheckDate(sPrice.Date);
                    //ctx.SubDisplayText = sPrice.PriceGetDate.ToString();

                }

                catch (ApplicationException e)
                {
                    var exText = e.Data["エラー種別"].ToString();
                    ctx.SubDisplayText = exText;
                }
            }
            else
            {
                ctx.SubDisplayText = "INPUT ERROR";
            }
        }
        
        /// <summary>
        /// ＮＹダウ平均を取得する
        /// </summary>
        /// <param name="Factx"></param>
        /// <param name="inputCode"></param>
        private void GetDow(ISubContext Factx)
        {
            var ctx = (StockContext)Factx;
            try
            {

                //TODO:テストの為の裏口設定を通れ
                var sPrice = DowUtil.GetDowPrice();

                //ＮＹダウ平均値取得成功時
                ctx.DisplayText = $"[DJI] {sPrice.Price.ToString("#,0.00")} JPY";

                //取得日時
                ctx.SubDisplayText = sPrice.Date.ToString();
                //TODO:後に取得時間への対応を入れよ
                //ctx.SubDisplayText = StockTimeUtil.IsClosingTime(sPrice);
            }

            catch (ApplicationException e)
            {
                var exText = e.Data["エラー種別"].ToString();
                ctx.SubDisplayText = exText;
            }
        }
    }
}
