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
                   
                    ctx.SubDisplayText = InTradingHours(sPrice);
                    //ctx.SubDisplayText = sPrice.Date.ToString();
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

        /// <summary>
        /// 取引時間内外による株価取得時間メッセ
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string InTradingHours(StockPrice sPrice)
        {
            //TODO:土・日曜や月曜日の9時以前を考慮する
            var youbi = (int)sPrice.Date.DayOfWeek;

            
            int day = -1;
            if (youbi == 0)
            { }

            else if (youbi == 1 && sPrice.Date.Hour < 9) day = -3;


            //取引時間外の時
            if (sPrice.Date.Hour < 9)
            {
                

                sPrice.Date.AddDays(day);
                return sPrice.Date.ToString("yyyy.MM.dd") + "オワリネ";
            }
            else if (sPrice.Date.Hour >= 15)
            {
                //15時以降の取得
                return sPrice.Date.ToString("yyyy.MM.dd") + "オワリネ";
            }
            else
            {
                //取引時間内の取得
                return sPrice.Date.ToString("yyyy.MM.dd HH:mm:ss");
            }
        }

    }
}
