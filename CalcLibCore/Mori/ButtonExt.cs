namespace CalcLib.Mori
{
    /// <summary>
    /// CalcButtonの拡張メソッド
    /// </summary>
    internal static class ButtonExt
    {

        internal static string ToNumberString(this CalcButton btn) => btn switch
        {
            CalcButton.Btn0 => "0",
            CalcButton.Btn1 => "1",
            CalcButton.Btn2 => "2",
            CalcButton.Btn3 => "3",
            CalcButton.Btn4 => "4",
            CalcButton.Btn5 => "5",
            CalcButton.Btn6 => "6",
            CalcButton.Btn7 => "7",
            CalcButton.Btn8 => "8",
            CalcButton.Btn9 => "9",
            CalcButton.BtnDot => ".",
            _ => ""
        };

        internal static string ToOperatorString(this CalcButton btn) => btn switch
        {
            CalcButton.BtnPlus => "+",
            CalcButton.BtnMinus => "-",
            CalcButton.BtnMultiple => "×",
            CalcButton.BtnDivide => "÷",
            _ => ""
        };


        internal static bool IsNumber(this CalcButton btn) => btn.IsBetween(CalcButton.Btn0, CalcButton.Btn9) || btn == CalcButton.BtnDot;

        internal static bool IsOperator(this CalcButton btn) => btn.IsBetween(CalcButton.BtnPlus, CalcButton.BtnMultiple);


        internal static bool IsEqual(this CalcButton btn)
        {
            return btn == CalcButton.BtnEqual;
        }

        internal static bool IsClear(this CalcButton btn) => btn == CalcButton.BtnClear;

        internal static bool IsBS(this CalcButton btn) => btn == CalcButton.BtnBS;

        internal static bool IsCE(this CalcButton btn) => btn == CalcButton.BtnClearEnd;

        internal static bool IsOmikuji(this CalcButton btn) => btn == CalcButton.BtnExt2;
        internal static bool IsStock(this CalcButton btn) => btn == CalcButton.BtnExt3;
        internal static bool IsOmikujiSelect(this CalcButton btn) => btn.IsBetween(CalcButton.Btn1, CalcButton.Btn4);

        internal static bool IsBetween(this CalcButton btn, CalcButton start, CalcButton end) => start <= btn && btn <= end;

    }
} 

