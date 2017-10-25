using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    public class Calculator : BaseApp, IApplication
    {
        /// <summary>
        /// 入力状態
        /// </summary>
        public enum State
        {
            Operator = 0,  // 演算子入力後
            Equal,         // イコール入力後
            Other,         // その他
            Fin,           // アプリ終了
        }

        /// <summary>
        /// 入力状態
        /// </summary>
        public State InputState { get; set; }

        /// <summary>
        /// 計算項目を入れておくQueue
        /// </summary>
        public List<CalcItem> Queue { get; private set; } = new List<CalcItem>();

        /// <summary>
        /// 計算項目
        /// </summary>
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
                switch (Item)
                {
                    case OPE_PLUS:
                    case OPE_MINUS:
                    case OPE_MULTIPLE:
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
                if (decimal.TryParse(Item, out result))
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
                switch (btn)
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
        /// 現状での計算結果を返す
        /// </summary>
        public decimal Calc()
        {
            if (Queue.Count < 1)
            {
                return 0;
            }

            decimal answer = Queue[0].ToDecimal();
            var tmpQueue = Queue.GetRange(1, Queue.Count - 1);
            string ope = "";
            foreach (var item in tmpQueue)
            {
                if (item.ToString() == CalcItem.OPE_EQUAL)
                {
                    break;
                }
                if (item.IsArithmeticOperator())
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
        /// 値と演算子を渡すことで計算してくれる関数
        /// </summary>
        private decimal SingleCalc(decimal value1, decimal value2, string ope)
        {
            switch (ope)
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

        /// <summary>
        /// アプリ実行
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public void Run(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcSvcYamamoto.CalcContextYamamoto;

            switch (btn)
            {
                // "＋"
                // "－"
                // "×"
                // "÷"
                case CalcButton.BtnPlus:
                case CalcButton.BtnMinus:
                case CalcButton.BtnMultiple:
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

                // "BS"
                case CalcButton.BtnBS:
                    BackSpaceProc(ctx, btn);
                    break;

                // "C"
                case CalcButton.BtnClear:
                    ClearProc(ctx, btn);
                    break;

                // "CE"
                case CalcButton.BtnClearEnd:
                    ClearEndProc(ctx, btn);
                    break;

                // "%"
                case CalcButton.BtnExt1:
                    PercentProc(ctx, btn);
                    break;

                // "1"
                // "2"
                // "3"
                // "4"
                // "5"
                // "6"
                // "7"
                // "8"
                // "9"
                // "0"
                case CalcButton.Btn1:
                case CalcButton.Btn2:
                case CalcButton.Btn3:
                case CalcButton.Btn4:
                case CalcButton.Btn5:
                case CalcButton.Btn6:
                case CalcButton.Btn7:
                case CalcButton.Btn8:
                case CalcButton.Btn9:
                case CalcButton.Btn0:
                    NumProc(ctx, btn);
                    break;

                // "おみくじ"
                case CalcButton.BtnExt2:
                    ToOmikujiMode(ctx, btn);
                    InputState = State.Fin;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ドットを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void DotProc(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            // 演算子またはイコールが押されていれば表示値を消しておく
            if (InputState == State.Operator || InputState == State.Equal)
            {
                DisplayTextClear(ctx);
            }

            if (!string.IsNullOrWhiteSpace(ctx.DisplayText) && ctx.DisplayText.IndexOf(".") > 0)
            {
                // すでにドットが入力されている場合は入力不可
                return;
            }

            // 一桁もない場合は0を付与しておく
            if (string.IsNullOrWhiteSpace(ctx.DisplayText))
            {
                ctx.DisplayText += "0";
            }
            ctx.DisplayText += CalcItem.GetBtnString(btn);

            // その他ボタン押下後の状態へ遷移
            InputState = State.Other;
        }

        /// <summary>
        /// 数字を押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void NumProc(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            // 演算子またはイコールが押されていれば表示値を消しておく
            if (InputState == State.Operator || InputState == State.Equal)
            {
                DisplayTextClear(ctx);
            }

            // TODO: 数字を押し続けると落ちるため、制御する必要あり
            // 桁数が最大に到達している場合は入力不可
            //if (ctx.DisplayText.Where(x => char.IsDigit(x)).Count() >= 13)
            //{
            //    return;
            //}

            var num = decimal.Parse(ctx.DisplayText.Replace(",", "") + CalcItem.GetBtnString(btn));
            ctx.DisplayText = num.ToCommaString();

            // その他ボタン押下後の状態へ遷移
            InputState = State.Other;
        }

        /// <summary>
        /// 演算子ボタンを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void OperatorProc(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            // 前回が演算子の場合は上書き
            if (InputState == State.Operator)
            {
                // [HACK]
                //    一番最後を削除できる拡張メソッドを作ったらきれいに書けそう
                Queue.RemoveAt(Queue.Count - 1);
                Queue.Add(new CalcItem(CalcItem.GetBtnString(btn)));

                // 計算過程を反映
                ctx.SubDisplayText = GetCalcProcess();
                return;
            }

            // 入力値をリストに追加
            if (string.IsNullOrWhiteSpace(ctx.DisplayText))
            {
                // 入力値がない場合は0を入れておく
                Queue.Add(new CalcItem("0"));
            }
            else
            {
                // 現在の入力値を追加
                Queue.Add(new CalcItem(decimal.Parse(ctx.DisplayText).ToString()));
            }
            Queue.Add(new CalcItem(CalcItem.GetBtnString(btn)));

            // 計算結果を表示
            var answer = Calc();
            ctx.DisplayText = answer.ToDisplayText();

            // 計算過程を反映
            var process = GetCalcProcess();
            ctx.SubDisplayText = process;

            // 演算子ボタン押下後の状態へ遷移
            InputState = State.Operator;
        }

        /// <summary>
        /// 最終結果計算
        /// </summary>
        /// <param name="ctx"></param>
        private void EqualProc(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            // 入力値をリストに追加
            if (string.IsNullOrWhiteSpace(ctx.DisplayText))
            {
                // 入力値がない場合は0を入れておく
                Queue.Add(new CalcItem("0"));
            }
            else
            {
                Queue.Add(new CalcItem(ctx.DisplayText));
            }
            Queue.Add(new CalcItem(CalcItem.GetBtnString(btn)));

            // 計算結果を表示
            var answer = Calc();
            ctx.DisplayText = answer.ToDisplayText();

            // 計算過程クリア
            ctx.SubDisplayText = "";
            Queue.Clear();

            // イコールボタン押下後の状態へ遷移
            InputState = State.Equal;
        }

        /// <summary>
        /// パーセントを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void PercentProc(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            // サブディスプレイの内容を計算
            var subResult = Calc();

            // 入力されている値を%として計算する
            var answer = subResult * (decimal.Parse(ctx.DisplayText) / 100);

            // 表示
            ctx.DisplayText = answer.ToDisplayText();
            ctx.SubDisplayText += answer.CutTrailingZero().ToString();
        }

        /// <summary>
        /// BackSpaceを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void BackSpaceProc(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            // 演算子またはイコールが押されていればそのままにしておく
            if (InputState == State.Operator || InputState == State.Equal)
            {
                return;
            }

            if (!string.IsNullOrEmpty(ctx.DisplayText))
            {
                var tmpText = ctx.DisplayText.Remove(ctx.DisplayText.Length - 1);
                ctx.DisplayText = decimal.Parse(tmpText).ToDisplayText();
            }

            // HACK 拡張メソッドで破壊的メソッドを定義出来たらこんな書き方にしたい
            //ctx.DisplayText.BackSpace();
        }

        /// <summary>
        /// ClearEndを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void ClearProc(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            DisplayTextClear(ctx, "0");
            SubDisplayTextClear(ctx);
        }

        /// <summary>
        /// ClearEndを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void ClearEndProc(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            DisplayTextClear(ctx, "0");
        }

        /// <summary>
        /// 表示値クリア
        /// </summary>
        private void DisplayTextClear(CalcSvcYamamoto.CalcContextYamamoto ctx, string clearText = "")
        {
            ctx.DisplayText = clearText;
        }

        /// <summary>
        /// 表示値クリア
        /// </summary>
        private void SubDisplayTextClear(CalcSvcYamamoto.CalcContextYamamoto ctx)
        {
            Queue.Clear();
            ctx.SubDisplayText = "";
        }

    }
}
