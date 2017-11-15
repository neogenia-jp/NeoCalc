using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    public class StockApp : BaseApp
    {
        /// <summary>
        /// 入力状態
        /// </summary>
        public enum State
        {
            Init = 0,      // 初期化
            ShowStock,     // 株価表示
            Error,         // 株価表示失敗
            Fin,           // 証券コード取得アプリ終了
        }

        /// <summary>
        /// 入力状態
        /// </summary>
        public State InputState { get; set; } = State.Init;

        /// <summary>
        /// 最初の証券コードを保持しておく場所
        /// </summary>
        private string InitialCode { get; set; } = "";

        /// <summary>
        /// アプリ実行
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public override void Run(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcSvcYamamoto.CalcContextYamamoto;

            if(InputState == State.Init)
            {
                Init(ctx);
                if(InputState != State.Error)
                {
                    CompanyStockPrice(ctx, InitialCode);
                }
                return;
            }

            switch (btn)
            {
                // "+"
                case CalcButton.BtnPlus:
                    N225(ctx);
                    break;
                // "-"
                case CalcButton.BtnMinus:
                    break;
                // "="
                case CalcButton.BtnEqual:
                    CompanyStockPrice(ctx, InitialCode);
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
                    ToCaliculatorMode();
                    InputState = State.Fin;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <param name="ctx"></param>
        private void Init(CalcSvcYamamoto.CalcContextYamamoto ctx)
        {
            var text = ctx.DisplayText.Replace(",", "");
            if (!IsShokenCode(text))
            {
                ctx.SubDisplayText = "INPUT ERROR";
                ctx.DisplayText = "";
                InputState = State.Error;
                return;
            }

            InitialCode = text;
            return;
        }

        /// <summary>
        /// 通常の株価取得
        /// </summary>
        /// <param name="ctx"></param>
        private void CompanyStockPrice(CalcSvcYamamoto.CalcContextYamamoto ctx, string code)
        {
            Util.StockPrice sp;
            try
            {
                sp = Util.StockUtilYamamotoWrapper.GetInstance().GetStockPrice(code);
            }
            catch(Exception ex) when (ex.InnerException is System.Net.WebException || ex.InnerException is Util.StockUtilYamamoto.ScrapingException)
            {
                ctx.SubDisplayText = "SCRAPING ERROR";
                ctx.DisplayText = "";
                InputState = State.Error;
                return;
            }
            ctx.DisplayText = $"[{code}] {sp.Price.ToCommaString()} JPY";

            // 終値確認
            // HACK: 証券取引所の営業時間は9時から15時としている
            if(sp.Date.Hour < 9 || sp.Date.Hour > 14)
            {
                var date = sp.Date;
                if(sp.Date.Hour < 9)
                {
                    // 9時より前は前日
                    date = date.AddDays(-1);
                }
                ctx.SubDisplayText = $"{date.ToString("yyyy.MM.dd")} オワリネ";
            }
            else
            {
                ctx.SubDisplayText = $"{sp.Date.ToString("yyyy.MM.dd")} {sp.Date.ToString("hh:mm")}";
            }

            InputState = State.ShowStock;
        }

        /// <summary>
        /// 日経平均株価取得
        /// </summary>
        /// <param name="ctx"></param>
        private void N225(CalcSvcYamamoto.CalcContextYamamoto ctx)
        {
            // 株価取得成功時にしか表示しないため、何もせずに終了
            if (InputState == State.Error) return;

            Util.StockPrice sp;
            try
            {
                sp = Util.StockUtilYamamotoWrapper.GetInstance().GetStockPrice(Util.StockUtilYamamoto.N225_CODE);
            }
            catch(Exception ex) when (ex.InnerException is System.Net.WebException || ex.InnerException is Util.StockUtilYamamoto.ScrapingException)
            {
                ctx.SubDisplayText = "SCRAPING ERROR";
                ctx.DisplayText = "";
                return;
            }

            ctx.DisplayText = $"[N225] {sp.Price.ToCommaString()} JPY";
            InputState = State.ShowStock;
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
