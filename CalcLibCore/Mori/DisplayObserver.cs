using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CalcLib.Mori
{
    // Subjectインターフェース
    internal interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    // Observerインターフェース
    internal interface IObserver
    {
        void Update(ISubject subject);
    }

    internal class DisplayObserver : IObserver
    {
        private readonly CalcContextExtend _context;

        public DisplayObserver(CalcContextExtend context)
        {
            _context = context;
        }

        public void Update(ISubject subject)
        {
            if (subject is CalcContextExtend context)
            {
                UpdateMainDisplay();
                UpdateSubDisplay();
            }
        }

        private string TryFormatNumber(string value)
        {
            if (decimal.TryParse(value, out var number))
            {
                return number.ToString("#,##0.#############");
            }
            return value;
        }

        private void UpdateMainDisplay()
        {
            var source = _context.DisplaySource;
            var buffer = source.MainText;
            var mode = source.Mode;

            // 計算中のバッファを表示用にフォーマットする
            if (string.IsNullOrEmpty(buffer)) { _context.DisplayText = "0"; return; }

            // 計算確定後は、バッファをそのまま表示する
            if (mode is UIMode.CalcDefault)
            {
                _context.DisplayText = TryFormatNumber(buffer);
                return;
            }

            // 以下、小数点含む入力中の処理
            string intPart;
            string? fracPart = null;

            // 小数点を含む場合は、整数部と小数部に分ける
            if (buffer.Contains('.'))
            {
                var parts = buffer.Split('.');
                intPart = parts[0];
                fracPart = parts[1];
            }
            else
            {
                // 小数点を含まない場合はbufferをそのままもらう
                intPart = buffer;
            }
            
            // 0.0にするケース
            if (intPart.Length == 0) intPart = "0";

            // 整数部をカンマ区切りフォーマットする
            string formattedIntPart;
            if(decimal.TryParse(intPart, out var intNumber))
            {
                formattedIntPart = intNumber.ToString("#,##0");
            }
            else
            {
                formattedIntPart = intPart;
            }
            
            // 再合成して表示
            if (fracPart != null)
            {
                _context.DisplayText = $"{formattedIntPart}.{fracPart}";
            }
            else
            {
                _context.DisplayText = formattedIntPart;
            }
        }


        private void UpdateSubDisplay()
        {
            // TODO: 3項以降の表示は削るよう整形したい
            // var formattedHistory = calculator.DisplayHistory.Select(FormatIfNumber);
            _context.SubDisplayText = _context.DisplaySource.SubText;
        }
    }
}
