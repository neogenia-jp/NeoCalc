using System;
namespace CalcLib.Yamamoto2
{
	public static class Consts
	{
	    /// <summary>
	    /// 電卓のボタンを表すEnum
	    /// </summary>
	    public static readonly Dictionary<CalcButton, string> CalcButtonText = new Dictionary<CalcButton, string>()
	    {
			{ CalcButton.Btn0, "0" },
			{ CalcButton.Btn1, "1" },
			{ CalcButton.Btn2, "2" },
			{ CalcButton.Btn3, "3" },
			{ CalcButton.Btn4, "4" },
			{ CalcButton.Btn5, "5" },
			{ CalcButton.Btn6, "6" },
			{ CalcButton.Btn7, "7" },
			{ CalcButton.Btn8, "8" },
			{ CalcButton.Btn9, "9" },
			{ CalcButton.BtnDot, "." },

			{ CalcButton.BtnEqual, "=" },
			{ CalcButton.BtnPlusMinus, "" },

			{ CalcButton.BtnPlus, "+" },
			{ CalcButton.BtnMinus, "-" },
			{ CalcButton.BtnDivide, "/" },
			{ CalcButton.BtnMultiple, "×" },

			{ CalcButton.BtnClear, "" },
			{ CalcButton.BtnClearEnd, "" },
			{ CalcButton.BtnBS, "" },

			{ CalcButton.BtnExt1, "" },
			{ CalcButton.BtnExt2, "" },
			{ CalcButton.BtnExt3, "" },
			{ CalcButton.BtnExt4, "" },
	    };
	}
}

