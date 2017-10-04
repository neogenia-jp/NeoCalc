using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    public class Calculator
    {
        public List<CalcItem> Queue { get; private set; }

        public class CalcItem
        {
            public const string ZERO = "0";
            public const string ONE = "1";
            public const string TWO = "2";
            public const string THREE = "3";
            public const string FOUR = "4";
            public const string FIVE = "5";
            public const string SIX = "6";
            public const string SEVEN = "7";
            public const string EIGHT = "8";
            public const string NINE = "9";
            public const string DOT = ".";
            public const string OPE_PLUS = "＋";
            public const string OPE_MINUS = "－";
            public const string OPE_MULTIPLE = "×";
            public const string OPE_DIVIDE = "÷";
            public const string OPE_PLUS_MINUS = "±";
            public const string OPE_EQUAL = "＝";

            /// <summary>
            /// 項目
            /// </summary>
            public string Item { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CalcItem(string val)
            {
                Item = val;
            }

            /// <summary>
            /// 算術演算子かどうかを判定する
            /// </summary>
            public bool IsArithmeticOperator()
            {
                switch(Item)
                {
                    case OPE_PLUS:
                        return true;
                    case OPE_MINUS:
                        return true;
                    case OPE_MULTIPLE:
                        return true;
                    case OPE_DIVIDE:
                        return true;
                    default:
                        return false;
                }
            }

            /// <summary>
            /// 数値への変換
            /// </summary>
            public double ToDouble()
            {
                double result;
                if(double.TryParse(Item, out result))
                {
                    return result;
                }
                return 0;
            }

            /// <summary>
            /// 文字列への変換
            /// </summary>
            public override string ToString()
            {
                return Item.ToString();
            }

            /// <summary>
            /// ボタンに対応する文字を返す
            /// </summary>
            /// <param name="btn"></param>
            /// <returns></returns>
            public static string GetBtnString(CalcButton btn)
            {
                switch(btn)
                {
                    case CalcButton.Btn0:
                        return ZERO;
                    case CalcButton.Btn1:
                        return ONE;
                    case CalcButton.Btn2:
                        return TWO;
                    case CalcButton.Btn3:
                        return THREE;
                    case CalcButton.Btn4:
                        return FOUR;
                    case CalcButton.Btn5:
                        return FIVE;
                    case CalcButton.Btn6:
                        return SIX;
                    case CalcButton.Btn7:
                        return SEVEN;
                    case CalcButton.Btn8:
                        return EIGHT;
                    case CalcButton.Btn9:
                        return NINE;
                    case CalcButton.BtnDot:
                        return DOT;
                    case CalcButton.BtnPlus:
                        return OPE_PLUS;
                    case CalcButton.BtnMinus:
                        return OPE_MINUS;
                    case CalcButton.BtnMultiple:
                        return OPE_MULTIPLE;
                    case CalcButton.BtnDivide:
                        return OPE_DIVIDE;
                    case CalcButton.BtnPlusMinus:
                        return OPE_PLUS_MINUS;
                    case CalcButton.BtnEqual:
                        return OPE_EQUAL;
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Calculator()
        {
            Queue = new List<CalcItem>();
        }

        /// <summary>
        /// 現状での計算結果を返す
        /// </summary>
        public double Calc()
        {
            if(Queue.Count < 1)
            {
                return 0;
            }

            double answer = Queue[0].ToDouble();
            var tmpQueue = Queue.GetRange(1, Queue.Count - 1);
            string ope = "";
            foreach(var item in tmpQueue)
            {
                if(item.ToString() == CalcItem.OPE_EQUAL)
                {
                    break;
                }
                if(item.IsArithmeticOperator())
                {
                    // 算術演算子の場合は保持しておく
                    ope = item.ToString();
                    continue;
                }
                answer = SingleCalc(answer, item.ToDouble(), ope);
            }

            return answer;
        }

        /// <summary>
        /// 現状での計算過程を返す
        /// </summary>
        public string GetCalcProcess()
        {
            string process = "";
            foreach(var item in Queue)
            {
                process += item.ToString();
            }
            return process;
        }

        /// <summary>
        /// 要素追加
        /// </summary>
        public void Add(CalcItem item)
        {
            Queue.Add(item);
        }

        /// <summary>
        /// 要素全クリア
        /// </summary>
        public void Clear()
        {
            Queue.Clear();
        }

        /// <summary>
        /// 値と演算子を渡すことで計算してくれる関数
        /// </summary>
        private double SingleCalc(double value1, double value2, string ope)
        {
            switch(ope)
            {
                case CalcItem.OPE_PLUS:
                    return value1 + value2;
                case CalcItem.OPE_MINUS:
                    return value1 - value2;
                case CalcItem.OPE_MULTIPLE:
                    return value1 * value2;
                case CalcItem.OPE_DIVIDE:
                    return value1 / value2;
                default:
                    return 0;
            }
        }
    }
}
