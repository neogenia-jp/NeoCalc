using System;
using CalcLib;
using CalcLibCore.Tomida.Commands;

namespace CalcLibCore.Tomida
{
	public class CalcConstants
	{
        public enum State
        {
            InputLeft,
            InputOperator,
            InputRight,
            InputComplete,
            InputEqual
        }

        public static List<CalcButton> systemButtons = new List<CalcButton>
        {
            CalcButton.BtnBS,
            CalcButton.BtnClear,
            CalcButton.BtnClearEnd,
        };

        public static List<CalcButton> operators = new List<CalcButton>
        {
            CalcButton.BtnPlus,
            CalcButton.BtnMinus,
            CalcButton.BtnMultiple,
            CalcButton.BtnDivide,
            CalcButton.BtnEqual
        };
        public static List<CalcButton> numbers = new List<CalcButton>
        {
            CalcButton.Btn1,
            CalcButton.Btn2,
            CalcButton.Btn3,
            CalcButton.Btn4,
            CalcButton.Btn5,
            CalcButton.Btn6,
            CalcButton.Btn7,
            CalcButton.Btn8,
            CalcButton.Btn9,
            CalcButton.Btn0,
            CalcButton.BtnDot
        };
        public static Dictionary<CalcButton, string> DisplayStringDic = new Dictionary<CalcButton, string>() {
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
            { CalcButton.BtnPlus, "+" },
            { CalcButton.BtnMinus, "-" },
            { CalcButton.BtnMultiple, "×" },
            { CalcButton.BtnDivide, "÷" },
            { CalcButton.BtnEqual, "=" },
        };

        public static Dictionary<CalcButton, ICalcOperator> OperatorCommandDic = new Dictionary<CalcButton, ICalcOperator>
        {
            {CalcButton.BtnPlus, new AddOperator() },
            {CalcButton.BtnMinus, new SubtractOperator() },
            {CalcButton.BtnMultiple, new MultiplyOperator() },
            {CalcButton.BtnDivide, new DivideOperator() }
        };
    }
}

