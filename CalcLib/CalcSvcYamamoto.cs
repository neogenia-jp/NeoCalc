using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    internal class CalcSvcYamamoto : ICalcSvcEx
    {
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
            ctx.DisplayText += Calculator.CalcItem.GetBtnString(btn);

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
            // 入力値をリストに追加
            if (string.IsNullOrWhiteSpace(ctx.DisplayText))
            {
                // 入力値がない場合は0を入れておく
                ctx.Cal.Add(new Calculator.CalcItem("0"));
            }
            else
            {
                // 現在の入力値を計算
                ctx.Cal.Add(new Calculator.CalcItem(ctx.DisplayText));
            }
            ctx.Cal.Add(new Calculator.CalcItem(Calculator.CalcItem.GetBtnString(btn)));

            // 計算結果を表示
            var answer = ctx.Cal.Calc();
            ctx.DisplayText = answer.ToString();

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
            ctx.DisplayText = answer.ToString();

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
            var answer = subResult * (double.Parse(ctx.DisplayText) / 100);

            // 表示
            ctx.DisplayText = answer.ToString();
            ctx.SubDisplayText += answer.ToString();
        }

    }
}
