using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CalcLib.Maeda.Basis;
using CalcLib.Maeda.Dispatcher;
using CalcLib.Util;

namespace CalcLib.Maeda.Finance
{
    /// <summary>
    /// ファイナンスサービスのためのコンテキスト
    /// </summary>
    internal class FinanceContext : ICalcContext
    {
        /// <summary>
        /// 証券コード4桁
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 通貨コード
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 株価
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// いつ時点の株価か
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get; set; }

        public string DisplayText => ErrorMessage == null
            ? $"[{StockCode}] {Price} {Currency}"  // TODO カンマ編集
            : StockCode;

        public string SubDisplayText => ErrorMessage ?? "";

        public SvcState State { get; set; }
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
        }

        public override bool TryButtonClick(FinanceContext ctx, CalcButton btn)
        {
            ctx.ErrorMessage = null;  // エラークリア

            switch (btn)
            {
                case CalcButton.BtnExt1:
                    if (ctx.State != SvcState.Unknown) return false;
                    // 初回処理
                    try
                    {
                        ctx.StockCode = CheckInput(ctx.StockCode);        // 入力チェック（エラー時は例外が出る）
                        var sp = UtilActivator.Get<IStockUtil>().GetStockPrice(ctx.StockCode);  // 株価取得
                        ctx.Price = sp.Price;  // 株価
                        ctx.Currency = "JPY";  // 通貨は JPY 固定とする
                        ctx.Date = sp.Date;    // 日時
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
                    ctx.State = SvcState.Finished;
                    break;
                default:
                    if (ctx.State == SvcState.Finished) return false;
                    // 2回目以降
                    // FIXME
                    break;
            }

            return true;
        }

        protected override void OnExitSvc(FinanceContext ctx)
        {
            ctx.State = SvcState.Unknown;
        }
    }
}
