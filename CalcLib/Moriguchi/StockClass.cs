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

        public void Init(ISubContext Factx)
        {
            var ctx = (StockContext)Factx;

            var code = CalcSvcMoriguchi.ContextMoriguchi.Disp;

            //４桁だったら株価取得へ
            if (code.Length == 4)
            {
                try
                {
                    var sPrice = StockUtil.GetStockPrice(code);

                    //株価取得成功時
                    ctx.DisplayText = $"[{code}] {sPrice.Price.ToString()} JPY";
                    ctx.SubDisplayText = sPrice.Date.ToString();
                }
                catch (NullReferenceException)
                {
                    ctx.SubDisplayText = "該当する銘柄は見つかりませんでした";
                }
                catch (Exception)
                {
                    ctx.SubDisplayText = "SCRAPING ERROR";
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

        ///// <summary>
        ///// 取引時間内での株価取得かどうかの判定
        ///// </summary>
        ///// <param name="date"></param>
        ///// <returns></returns>
        //public bool InTradingHours(DateTime date)
        //{
        //    var OpeningTime = new DateTime();
        //    var ClosingTime = new DateTime();

        //    if (date
        //}

    }
}
