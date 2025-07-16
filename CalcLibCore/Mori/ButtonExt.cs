namespace CalcLib
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


        internal static bool IsNumber(this CalcButton btn)
        {
            return (CalcButton.Btn0 <= btn && btn <= CalcButton.Btn9) || btn == CalcButton.BtnDot;
        }

        internal static bool IsOperator(this CalcButton btn)
        {
            return CalcButton.BtnPlus <= btn && btn <= CalcButton.BtnMultiple;
        }


        internal static bool IsEqual(this CalcButton btn)
        {
            return btn == CalcButton.BtnEqual;
        }

        internal static bool IsClear(this CalcButton btn)
        {
            return CalcButton.BtnClear <= btn && btn <= CalcButton.BtnClearEnd;
        }

        internal static bool IsBS(this CalcButton btn)
        {
            return btn == CalcButton.BtnBS;
        }

    }
} 

