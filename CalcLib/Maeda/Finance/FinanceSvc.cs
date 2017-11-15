using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CalcLib.Maeda.Basis;
using CalcLib.Maeda.Dispatcher;
using CalcLib.Maeda.Util;

namespace CalcLib.Maeda.Finance
{
    public class FinanceInfo : CalcLib.Util.StockPrice
    {
        /// <summary>
        /// 通貨コード
        /// </summary>
        public string Currency { get; set; }

        public override string ToString() => $"[{Code}] {Price} {Currency}";

        /// <summary>
        /// 表示用文字列のフォーマット
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string DisplayFormat(string code) => $"[{code}] {string.Format("{0:#,0.##}", Price)} {Currency}";

        static TimeZoneInfo JST = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

        static TimeZoneInfo EST = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        /// <summary>
        /// 表示用の日付フォーマット
        /// </summary>
        /// <returns></returns>
        public string DisplayDate()
        {
            var owarine = false;
            var now = TimeCop.GetCurrentTime();
            if (Currency == "JPY" && TimeZoneInfo.ConvertTime(now, JST).Hour >= 15)
            {
                // 日本時間の15時以降なら証券取引所の営業終了とみなす
                owarine = true;
            }
            else if (Currency == "USD" && TimeZoneInfo.ConvertTime(now, EST).Hour > 17)
            {
                // 日本時間の15時以降なら証券取引所の営業終了とみなす
                owarine = true;
            }
            return Date.ToString($"yyyy.MM.dd {(owarine ? "オワリネ" : "HH:mm")}");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stockPrice"></param>
        /// <param name="currency"></param>
        public FinanceInfo(CalcLib.Util.StockPrice stockPrice, string currency) : base(stockPrice.Code, stockPrice.Price, stockPrice.Date)
        {
            Currency = currency;
        }
    }

    /// <summary>
    /// ファイナンスサービスのためのコンテキスト
    /// </summary>
    internal class FinanceContext : ICalcContext
    {
        public FinanceInfo Info { get; set; }

        /// <summary>
        /// 証券コード4桁
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get; set; }

        public string DisplayText => ErrorMessage == null
            ? Info?.DisplayFormat(StockCode) ?? ""
            : StockCode;

        public string SubDisplayText => ErrorMessage ?? Info.DisplayDate();

        public SvcState State { get; set; }

        public CalcButton LastBtn { get; set; }
    }

    /// <summary>
    /// ファイナンスサービス
    /// </summary>
    internal class FinanceSvc : SvcBase<FinanceContext>
    {
        public override string GetExtButtonText(int num) => num == 1 ? "株価" : null;

        internal override FinanceContext _CreateContext() => new FinanceContext();

        internal string CheckInput(string code)
        {
            var mr = Regex.Match(code, @"^\d,?\d\d\d$");
            if (!mr.Success) throw new ApplicationException("INPUT ERROR");
            return code.Replace(",", "");
        }

        public override void OnEnter(FinanceContext ctx, SvcSwichedEventArg arg)
        {
            ctx.StockCode = arg.PrevCtx.DisplayText;
            ctx.State = SvcState.Initialized;
        }

        private bool DoProcess(CalcButton btn, FinanceContext ctx)
        {
            switch (btn)
            {
                case CalcButton.BtnPlus:
                    // 日経平均株価
                    {
                        var sp = UtilActivator.Get<IStockUtil>().GetNikkei225();  // 日経平均株価取得
                        ctx.StockCode = "N225";
                        ctx.Info = new FinanceInfo(sp, "JPY");
                        ctx.LastBtn = btn;
                    }
                    return true;
                case CalcButton.BtnMinus:
                    // NYダウ平均
                    {
                        var sp = UtilActivator.Get<IStockUtil>().GetNyDow();  // NY平均取得
                        ctx.StockCode = "DJI";
                        ctx.Info = new FinanceInfo(sp, "USD");
                        ctx.LastBtn = btn;
                    }
                    return true;
                case CalcButton.BtnExt1:
                    // 株価取得
                    {
                        ctx.StockCode = CheckInput(ctx.StockCode);        // 入力チェック（エラー時は例外が出る）
                        var sp = UtilActivator.Get<IStockUtil>().GetStockPrice(ctx.StockCode);  // 株価取得
                        ctx.Info = new FinanceInfo(sp, "JPY");
                        ctx.LastBtn = btn;
                    }
                    return true;
                case CalcButton.BtnEqual:
                    // 再取得
                    return DoProcess(ctx.LastBtn, ctx);
                case CalcButton.Btn0:
                case CalcButton.Btn1:
                case CalcButton.Btn2:
                case CalcButton.Btn3:
                case CalcButton.Btn4:
                case CalcButton.Btn5:
                case CalcButton.Btn6:
                case CalcButton.Btn7:
                case CalcButton.Btn8:
                case CalcButton.Btn9:
                    return false;
                default:
                    ctx.Info = null;  // クリア
                    return true;
            }
        }

        public override bool TryButtonClick(FinanceContext ctx, CalcButton btn)
        {
            ctx.ErrorMessage = null;  // エラークリア

            // 2回目以降のBtn1押下は、サービス終了とする
            if (ctx.State != SvcState.Initialized && btn == CalcButton.BtnExt1) return false;

            try
            {
                return DoProcess(btn, ctx);
            }
            catch (ApplicationException ex)
            {
                ctx.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                ctx.ErrorMessage = "SCRAPING ERROR";
                Debugger.Break();
            }
            finally
            {
                ctx.State = SvcState.Running;
            }

            return true;
        }

        protected override void OnExitSvc(FinanceContext ctx)
        {
            ctx.State = SvcState.Unknown;
        }
    }
}
