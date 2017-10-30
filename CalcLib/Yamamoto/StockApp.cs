using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcLib.Common.Stock;

namespace CalcLib.Yamamoto
{
    public class StockApp : BaseApp, IApplication
    {
        /// <summary>
        /// 入力状態
        /// </summary>
        public enum State
        {
            Init = 0,      // 初期化
            ShowStock,     // 株価表示
            Fin,           // 証券コード取得アプリ終了
        }

        /// <summary>
        /// 入力状態
        /// </summary>
        public State InputState { get; set; } = State.Init;

        /// <summary>
        /// アプリ実行
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public void Run(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcSvcYamamoto.CalcContextYamamoto;

            if(InputState == State.Init)
            {
                Init(ctx);
                InputState = State.ShowStock;
                return;
            }

            switch (btn)
            {
                // "+"
                case CalcButton.BtnPlus:
                    break;
                // "-"
                case CalcButton.BtnMinus:
                    break;
                // "="
                case CalcButton.BtnEqual:
                    break;

                // 数字
                case CalcButton.Btn1:
                case CalcButton.Btn2:
                case CalcButton.Btn3:
                case CalcButton.Btn4:
                case CalcButton.Btn5:
                case CalcButton.Btn6:
                case CalcButton.Btn7:
                case CalcButton.Btn8:
                case CalcButton.Btn9:
                case CalcButton.Btn0:
                    ToCaliculatorMode(ctx, btn);
                    InputState = State.Fin;
                    return;

                default:
                    break;
            }

            ctx.BeforeMode = ctx.Mode;
            ctx.Mode = CalcSvcYamamoto.CalcContextYamamoto.AppMode.Stock;
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <param name="ctx"></param>
        private void Init(CalcSvcYamamoto.CalcContextYamamoto ctx)
        {
            var stockSvc = StockSvcFactory.Create();

            var text = ctx.DisplayText.Replace(",", "");
            if (!IsShokenCode(text))
            {
                ctx.SubDisplayText = "INPUT ERROR";
                return;
            }

            decimal result = 0;
            var shokenCd = int.Parse(text);
            try
            {
                result = stockSvc.Scrape(shokenCd);
            }
            catch
            {
                ctx.SubDisplayText = "SCRAPING ERROR";
                return;
            }

            ctx.DisplayText = $"[{shokenCd}] {result.ToCommaString()} JPY";
        }

        /// <summary>
        /// 証券コードかどうか確認する
        /// </summary>
        /// <param name="shokenCd"></param>
        /// <returns></returns>
        private bool IsShokenCode(string shokenCd)
        {
            int x;
            return int.TryParse(shokenCd, out x) && x.ToString().Length == 4;
        }

    }
}
