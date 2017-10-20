using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    internal class CalcSvcYamamoto : ICalcSvcEx
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
                public const string OPE_PLUS = "+";
                public const string OPE_MINUS = "-";
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
                public decimal ToDecimal()
                {
                    decimal result;
                    if(decimal.TryParse(Item, out result))
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
            public decimal Calc()
            {
                if(Queue.Count < 1)
                {
                    return 0;
                }

                decimal answer = Queue[0].ToDecimal();
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
                    answer = SingleCalc(answer, item.ToDecimal(), ope);
                    answer = Rounding(answer);
                }

                return answer;
            }

            /// <summary>
            /// 四捨五入を行う
            /// </summary>
            /// <param name="num"></param>
            /// <returns></returns>
            private decimal Rounding(decimal num)
            {
                // 少数第14位で四捨五入し、小数点第13位までとする
                return Math.Round(num, 13);
            }

            /// <summary>
            /// 現状での計算過程を返す
            /// </summary>
            public string GetCalcProcess()
            {
                return string.Join(" ", Queue);
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
            private decimal SingleCalc(decimal value1, decimal value2, string ope)
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
        class CalcContextYamamoto : CalcContext
        {
            /// <summary>
            /// 入力状態
            /// </summary>
            public enum State
            {
                Num = 0,    // 数字入力後
                Operator,   // 演算子入力後
                Equal       // イコール入力後
            }

            /// <summary>
            /// 入力状態
            /// </summary>
            public State InputState { get; set; }

            /// <summary>
            /// 入力値を入れておく場所
            /// </summary>
            public Calculator Cal { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CalcContextYamamoto()
            {
                Cal = new Calculator();
            }

            /// <summary>
            /// 表示値クリア
            /// </summary>
            public void DisplayTextClear()
            {
                DisplayText = "";
            }
        }
        
        public virtual ICalcContext CreateContext() => new CalcContextYamamoto();

        /// <summary>
        /// ボタンクリック時の動作（一番最初にここに入る）
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextYamamoto;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            switch (btn)
            {
                // "＋"
                case CalcButton.BtnPlus:
                    OperatorProc(ctx, btn);
                    break;

                // "－"
                case CalcButton.BtnMinus:
                    OperatorProc(ctx, btn);
                    break;

                // "×"
                case CalcButton.BtnMultiple:
                    OperatorProc(ctx, btn);
                    break;

                // "÷"
                case CalcButton.BtnDivide:
                    OperatorProc(ctx, btn);
                    break;

                // "＝"
                case CalcButton.BtnEqual:
                    EqualProc(ctx, btn);
                    break;

                // "."
                case CalcButton.BtnDot:
                    DotProc(ctx, btn);
                    break;

                // "%"
                case CalcButton.BtnExt1:
                    PercentProc(ctx, btn);
                    break;

                default:
                    NumProc(ctx, btn);
                    break;
            }
        }

        /// <summary>
        /// 拡張ボタンのテキストを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetExtButtonText(int num)
        {
            if (num == 1) return "%";
            return null;
        }

        /// <summary>
        /// ドットを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void DotProc(CalcContextYamamoto ctx, CalcButton btn)
        {
            if (!string.IsNullOrWhiteSpace(ctx.DisplayText) && ctx.DisplayText.IndexOf(".") > 0)
            {
                // すでにドットが入力されている場合は入力不可
                return;
            }

            // 演算子またはイコールが押されていれば表示値を消しておく
            if (ctx.InputState == CalcContextYamamoto.State.Operator || ctx.InputState == CalcContextYamamoto.State.Equal)
            {
                ctx.DisplayTextClear();
            }

            // 一桁もない場合は0を付与しておく
            if(string.IsNullOrWhiteSpace(ctx.DisplayText))
            {
                ctx.DisplayText += "0";
            }
            ctx.DisplayText += Calculator.CalcItem.GetBtnString(btn);
        }

        /// <summary>
        /// 数字を押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void NumProc(CalcContextYamamoto ctx, CalcButton btn)
        {
            // 演算子またはイコールが押されていれば表示値を消しておく
            if (ctx.InputState == CalcContextYamamoto.State.Operator || ctx.InputState == CalcContextYamamoto.State.Equal)
            {
                ctx.DisplayTextClear();
            }
            var num = decimal.Parse(ctx.DisplayText + Calculator.CalcItem.GetBtnString(btn));
            ctx.DisplayText = num.ToCommaString();

            // 数字ボタン押下後の状態へ遷移
            ctx.InputState = CalcContextYamamoto.State.Num;
        }

        /// <summary>
        /// 演算子ボタンを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void OperatorProc(CalcContextYamamoto ctx, CalcButton btn)
        {
            // 前回が演算子の場合は上書き
            if (ctx.InputState == CalcContextYamamoto.State.Operator)
            {
                // [HACK]
                //    一番最後を削除できる拡張メソッドを作ったらきれいに書けそう
                ctx.Cal.Queue.RemoveAt(ctx.Cal.Queue.Count - 1);
                ctx.Cal.Add(new Calculator.CalcItem(Calculator.CalcItem.GetBtnString(btn)));

                // 計算過程を反映
                ctx.SubDisplayText = ctx.Cal.GetCalcProcess();
                return;
            }

            // 入力値をリストに追加
            if (string.IsNullOrWhiteSpace(ctx.DisplayText))
            {
                // 入力値がない場合は0を入れておく
                ctx.Cal.Add(new Calculator.CalcItem("0"));
            }
            else
            {
                // 現在の入力値を追加
                ctx.Cal.Add(new Calculator.CalcItem(decimal.Parse(ctx.DisplayText).ToString()));
            }
            ctx.Cal.Add(new Calculator.CalcItem(Calculator.CalcItem.GetBtnString(btn)));

            // 計算結果を表示
            var answer = ctx.Cal.Calc();
            ctx.DisplayText = answer.CutTrailingZero().ToCommaString();

            // 計算過程を反映
            var process = ctx.Cal.GetCalcProcess();
            ctx.SubDisplayText = process;

            // 演算子ボタン押下後の状態へ遷移
            ctx.InputState = CalcContextYamamoto.State.Operator;
        }

        /// <summary>
        /// 最終結果計算
        /// </summary>
        /// <param name="ctx"></param>
        private void EqualProc(CalcContextYamamoto ctx, CalcButton btn)
        {
            // 入力値をリストに追加
            if (string.IsNullOrWhiteSpace(ctx.DisplayText))
            {
                // 入力値がない場合は0を入れておく
                ctx.Cal.Add(new Calculator.CalcItem("0"));
            }
            else
            {
                ctx.Cal.Add(new Calculator.CalcItem(ctx.DisplayText));
            }
            ctx.Cal.Add(new Calculator.CalcItem(Calculator.CalcItem.GetBtnString(btn)));

            // 計算結果を表示
            var answer = ctx.Cal.Calc();
            ctx.DisplayText = answer.CutTrailingZero().ToCommaString();

            // 計算過程クリア
            ctx.SubDisplayText = "";
            ctx.Cal.Clear();

            // イコールボタン押下後の状態へ遷移
            ctx.InputState = CalcContextYamamoto.State.Equal;
        }

        /// <summary>
        /// パーセントを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void PercentProc(CalcContextYamamoto ctx, CalcButton btn)
        {
            // サブディスプレイの内容を計算
            var subResult = ctx.Cal.Calc();

            // 入力されている値を%として計算する
            var answer = subResult * (decimal.Parse(ctx.DisplayText) / 100);

            // 表示
            ctx.DisplayText = answer.CutTrailingZero().ToCommaString();
            ctx.SubDisplayText += answer.CutTrailingZero().ToString();
        }
    }
}
