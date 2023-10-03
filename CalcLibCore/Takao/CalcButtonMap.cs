using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Takao
{
    internal static class CalcButtonMap
    {
        public static Dictionary<CalcButton, string> NumberMap = new Dictionary<CalcButton, string>()
        {
            { CalcButton.Btn0, "0"},
            { CalcButton.Btn1, "1"},
            { CalcButton.Btn2, "2"},
            { CalcButton.Btn3, "3"},
            { CalcButton.Btn4, "4"},
            { CalcButton.Btn5, "5"},
            { CalcButton.Btn6, "6"},
            { CalcButton.Btn7, "7"},
            { CalcButton.Btn8, "8"},
            { CalcButton.Btn9, "9"},
            { CalcButton.BtnDot, "."}
        };

        public static Dictionary<CalcButton, string> OperatorMap = new Dictionary<CalcButton, string>()
        {
            { CalcButton.BtnPlus, "+"},
            { CalcButton.BtnMinus, "-"},
            { CalcButton.BtnDivide, "/"},
            { CalcButton.BtnMultiple, "x"}
        };

        public static Dictionary<CalcButton, string> ExtMap = new Dictionary<CalcButton, string>()
        {
            { CalcButton.BtnExt1, "ex1"},
            { CalcButton.BtnExt2, "ex2"},
            { CalcButton.BtnExt3, "ex3"},
            { CalcButton.BtnExt4, "ex4"},
        };

        public static Dictionary<CalcButton, string> OtherMap = new Dictionary<CalcButton, string>()
        {

            { CalcButton.BtnEqual, "="},
            { CalcButton.BtnClear, "clear"},
            { CalcButton.BtnClearEnd, "clearEnd"},
            { CalcButton.BtnBS, "bs"},
        };

        public static Dictionary<CalcButton, string> Map = new Dictionary<CalcButton, string>()
        {
            { CalcButton.Btn0, "0"},
            { CalcButton.Btn1, "1"},
            { CalcButton.Btn2, "2"},
            { CalcButton.Btn3, "3"},
            { CalcButton.Btn4, "4"},
            { CalcButton.Btn5, "5"},
            { CalcButton.Btn6, "6"},
            { CalcButton.Btn7, "7"},
            { CalcButton.Btn8, "8"},
            { CalcButton.Btn9, "9"},
            { CalcButton.BtnDot, "."},
            { CalcButton.BtnPlus, "+"},
            { CalcButton.BtnMinus, "-"},
            { CalcButton.BtnDivide, "/"},
            { CalcButton.BtnMultiple, "x"},
            { CalcButton.BtnExt1, "ex1"},
            { CalcButton.BtnExt2, "ex2"},
            { CalcButton.BtnExt3, "ex3"},
            { CalcButton.BtnExt4, "ex4"},
            { CalcButton.BtnEqual, "="},
            { CalcButton.BtnClear, "clear"},
            { CalcButton.BtnClearEnd, "clearEnd"},
            { CalcButton.BtnBS, "bs"},
            { CalcButton.BtnPlusMinus, "+-"}
        };
    }
};
