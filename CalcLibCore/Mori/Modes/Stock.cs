using System;
using System.Text.RegularExpressions;
using CalcLib.Util;

namespace CalcLib.Mori
{
    // 株価取得モード
    internal class Stock
    {
        private const string _n225Code = "100000018";
        private const string _dowCode = "DJI";
        private string _main = string.Empty;
        private string _sub = string.Empty;
        private bool _hasGetPrice = false;
        private string? _currentCode = null; // 再取得用の銘柄コード
        private static readonly TimeSpan _tokyoOpen = new TimeSpan(9, 0, 0);
        private static readonly TimeSpan _tokyoClose = new TimeSpan(15, 0, 0);

        /// <summary>
        /// 株価取得モードの初期化
        /// </summary>
        /// <param name="calcSource">電卓の表示テキスト</param>
        internal void Init(DisplaySource calcSource)
        {
            _main = string.Empty;
            _sub = string.Empty;
            _hasGetPrice = false;
            _currentCode = null;

            var text = calcSource.MainText?.Trim() ?? string.Empty;

            if (!Regex.IsMatch(text, "^\\d{4}$"))
            {
                _sub = "INPUT ERROR";
                return;
            }

            try
            {
                var stockPrice = StockUtilMori.GetStockPrice(text);
                _main = $"[{stockPrice.Code}] {stockPrice.Price.ToString("#,0.00")} JPY";
                _sub = BuildDateTimeText();
                _hasGetPrice = true;
                _currentCode = stockPrice.Code;
            }
            catch (Exception)
            {
                _sub = "SCRAPING ERROR";
                _main = string.Empty;
                _hasGetPrice = false;
                _currentCode = null;
                return;
            }
        }

        // 取得成功後のキー反応
        internal void Accept(CalcButton btn)
        {
            if (!_hasGetPrice) return;

            switch (btn)
            {
                case CalcButton.BtnMinus:
                    ShowDow();
                    break;
                case CalcButton.BtnPlus:
                    ShowNikkei();
                    break;
                case CalcButton.BtnEqual:
                    Refresh();
                    break;
            }
        }

        // 現在表示中の指標を再取得
        internal void Refresh()
        {
            if (string.IsNullOrEmpty(_currentCode)) return; // 銘柄コードがない場合は何もしない
            try
            {
                if (_currentCode == _dowCode)
                {
                    var sp = StockUtilMori.GetDowPrice();
                    _main = $"[DJI] {sp.Price.ToString("#,0.00")} USD";
                }
                else if (_currentCode == _n225Code)
                {
                    var sp = StockUtilMori.GetStockPrice(_n225Code);
                    _main = $"[N225] {sp.Price.ToString("#,0.00")} JPY";
                }
                else
                {
                    var sp = StockUtilMori.GetStockPrice(_currentCode);
                    _main = $"[{sp.Code}] {sp.Price.ToString("#,0.00")} JPY";
                }
                _sub = BuildDateTimeText();
                _hasGetPrice = true;
            }
            catch (Exception)
            {
                _sub = "SCRAPING ERROR";
            }
        }

        // 表示をクリア
        internal void Clear()
        {
            _main = string.Empty;
        }

        private void ShowDow()
        {
            try
            {
                var dp = StockUtilMori.GetDowPrice();
                _main = $"[DJI] {dp.Price.ToString("#,0.00")} USD";
                _sub = BuildDateTimeText();
                _hasGetPrice = true;
                _currentCode = _dowCode;
            }
            catch (Exception)
            {
                _sub = "SCRAPING ERROR";
            }
        }

        private void ShowNikkei()
        {
            try
            {
                var sp = StockUtilMori.GetStockPrice(_n225Code);
                _main = $"[N225] {sp.Price.ToString("#,0.00")} JPY";
                _sub = BuildDateTimeText();
                _hasGetPrice = true;
                _currentCode = _n225Code;
            }
            catch (Exception)
            {
                _sub = "SCRAPING ERROR";
            }
        }

        internal DisplaySource RowDisplay()
        {
            return new DisplaySource(_main, _sub, UIMode.CalcDefault);
        }

        // サブディスプレイに表示する日時 (+オワリネ)
        private string BuildDateTimeText()
        {
            var nowJst = DateTime.Now;
            // 曜日と営業時間判定
            var isOpen = IsWeekday(nowJst.DayOfWeek) &&  _tokyoOpen <= nowJst.TimeOfDay && nowJst.TimeOfDay < _tokyoClose;
            if (isOpen)
            {
                return nowJst.ToString("yyyy.MM.dd HH:mm");
            }

            return $"{nowJst:yyyy.MM.dd} オワリネ";
        }

        private static bool IsWeekday(DayOfWeek day)
        {
            return day != DayOfWeek.Saturday && day != DayOfWeek.Sunday;
        }

    }
}
