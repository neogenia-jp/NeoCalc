using CalcLib.Mori;

namespace CalcLib.Mori.Display
{
    // 表示整形の契約。DisplaySource → DisplayView にフォーマットする
    internal interface IDisplayFormatter
    {
        DisplayView Format(DisplaySource source);
    }

    // 既定、電卓向けのフォーマット
    internal sealed class DefaultDisplayFormatter : IDisplayFormatter
    {

        public DisplayView Format(DisplaySource source)
        {
            var buffer = source.MainText ?? string.Empty;
            // TODO: サブディスプレイの計算式表示は、3項以降の表示は削るよう整形したい
            // テストは通るがWindows電卓は2項までしか表示しない

            // 計算中のバッファを表示用にフォーマットする
            if (string.IsNullOrEmpty(buffer))
            {
                return new DisplayView("0", source.SubText ?? string.Empty);
            }

            // 計算確定後は、バッファをそのまま表示する
            if (source.Mode == UIMode.CalcDefault)
            {
                return new DisplayView(TryFormatNumber(buffer), source.SubText ?? string.Empty);
            }

            // 以下、小数点含む入力中の処理（UIMode.CalcInputting）
            string intPart = buffer;
            string? fracPart = null;

            var dotIndex = buffer.IndexOf('.');
            if (dotIndex >= 0)
            {
                intPart = buffer.Substring(0, dotIndex);
                fracPart = (dotIndex + 1 < buffer.Length) ? buffer.Substring(dotIndex + 1) : string.Empty;
            }

            // 0.0にするケース
            if (intPart.Length == 0) intPart = "0";

            // 整数部をカンマ区切りフォーマットする
            string formattedIntPart;
            if (decimal.TryParse(intPart, out var intNumber))
            {
                formattedIntPart = intNumber.ToString("#,##0");
            }
            else
            {
                formattedIntPart = intPart; // 数値化不可の場合は素通し
            }

            // 再合成して表示
            var main = (fracPart != null) ? $"{formattedIntPart}.{fracPart}" : formattedIntPart;
            return new DisplayView(main, source.SubText ?? string.Empty);
        }

        private string TryFormatNumber(string value)
        {
            if (decimal.TryParse(value, out var number))
            {
                return number.ToString("#,##0.#############");
            }
            return value;
        }
    }
}
