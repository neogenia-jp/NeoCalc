using System;

namespace CalcLib.Yamamoto3.Extensions;

internal static class CalcButtonExt
{
    public static string ToDisplayString(this CalcButton btn)
    {
        return btn switch
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
            CalcButton.BtnPlus => "+",
            CalcButton.BtnMinus => "-",
            CalcButton.BtnDivide => "/",
            CalcButton.BtnMultiple => "×",
            CalcButton.BtnDot => ".",
            _ => throw new ArgumentOutOfRangeException(nameof(btn), btn, null)
        };
    }

    public static bool IsNumber(this CalcButton btn)
    {
        return btn == CalcButton.Btn0 || btn == CalcButton.Btn1 || btn == CalcButton.Btn2 ||
                btn == CalcButton.Btn3 || btn == CalcButton.Btn4 || btn == CalcButton.Btn5 ||
                btn == CalcButton.Btn6 || btn == CalcButton.Btn7 || btn == CalcButton.Btn8 ||
                btn == CalcButton.Btn9 || btn == CalcButton.BtnDot;
    }

    public static bool IsOperator(this CalcButton btn)
    {
        return btn == CalcButton.BtnPlus || btn == CalcButton.BtnMinus ||
                btn == CalcButton.BtnDivide || btn == CalcButton.BtnMultiple;
    }
}
