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
                    NY_DOW(ctx);
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

            // 時刻変換
            var date = ConvertDateTime(sp.Date, "JPY");

            // 表示
            ctx.DisplayText = $"[{code}] {sp.Price.ToCommaString()} JPY";
            ctx.SubDisplayText = string.Format("{0} {1}", 
                date.ToString("yyyy.MM.dd"),
                IsOwarine(date, "JPY") ? "オワリネ" : date.ToString("HH:mm"));

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

            // 時刻変換
            var date = ConvertDateTime(sp.Date, "JPY");

            // 表示
            ctx.DisplayText = $"[N225] {sp.Price.ToCommaString()} JPY";
            ctx.SubDisplayText = string.Format("{0} {1}", 
                date.ToString("yyyy.MM.dd"),
                IsOwarine(date, "JPY") ? "オワリネ" : date.ToString("HH:mm"));

            InputState = State.ShowStock;
        }

        /// <summary>
        /// NYダウ平均取得
        /// </summary>
        /// <param name="ctx"></param>
        private void NY_DOW(CalcSvcYamamoto.CalcContextYamamoto ctx)
        {
            // 株価取得成功時にしか表示しないため、何もせずに終了
            if (InputState == State.Error) return;

            Util.StockPrice sp;
            try
            {
                sp = Util.StockUtilYamamotoWrapper.GetInstance().GetStockPrice(Util.StockUtilYamamoto.NY_DOW_CODE);
            }
            catch(Exception ex) when (ex.InnerException is System.Net.WebException || ex.InnerException is Util.StockUtilYamamoto.ScrapingException)
            {
                ctx.SubDisplayText = "SCRAPING ERROR";
                ctx.DisplayText = "";
                return;
            }

            // 時刻変換
            var date = ConvertDateTime(sp.Date, "USD");

            // 表示
            ctx.DisplayText = $"[DJI] {sp.Price.ToCommaString()} USD";
            ctx.SubDisplayText = string.Format("{0} {1}", 
                date.ToString("yyyy.MM.dd"),
                IsOwarine(date, "USD") ? "オワリネ" : date.DateTime.ToString("HH:mm"));

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

        /// <summary>
        /// 通貨と時刻から終値かどうかを判定する
        /// </summary>
        /// <param name="time"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        private bool IsOwarine(DateTimeOffset time, string currency)
        {
            if(currency == "JPY")
            {
                return time.Hour >= 15 ? true : false;
            }
            if(currency == "USD")
            {
                return time.Hour >= 16 ? true : false;
            }
            throw new ArgumentException("不正な通貨が渡されました。");
        }

        /// <summary>
        /// UTCからの時刻に変換
        /// </summary>
        /// <param name="time"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        private DateTimeOffset ConvertDateTime(DateTime time, string currency)
        {
            DateTimeOffset dto = time;
            if(currency == "USD")
            {
                return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dto, "Eastern Standard Time");
            }
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dto, "Tokyo Standard Time");
        }

    }
}
