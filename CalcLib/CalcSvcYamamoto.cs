using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    internal class CalcSvcYamamoto : ICalcSvc
    {
        class CalcContextYamamoto : CalcContext
        {
            /// <summary>
            /// 演算子を保持しておく
            /// </summary>
            public CalcButton Operator { get; set; }

            /// <summary>
            /// 結果値を保持しておく
            /// </summary>
            public double ResultValue { get; set; }
            
            /// <summary>
            /// ディスプレイテキストをクリアするフラグ
            /// </summary>
            public bool DisplayTextClearFlag { get; set; }

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
                case CalcButton.BtnPlus:
                    OperatorProc(ctx, btn);
                    break;

                case CalcButton.BtnMinus:
                    OperatorProc(ctx, btn);
                    break;

                case CalcButton.BtnMultiple:
                    OperatorProc(ctx, btn);
                    break;

                case CalcButton.BtnDivide:
                    OperatorProc(ctx, btn);
                    break;

                case CalcButton.BtnEqual:
                    EqualProc(ctx);
                    break;

                default:
                    if(ctx.DisplayTextClearFlag)
                    {
                        ctx.DisplayText = "";
                        ctx.DisplayTextClearFlag = false;
                    }
                    ctx.DisplayText += ((int)btn).ToString();
                    break;
            }
        }

        /// <summary>
        /// 演算子ボタンを押されたときの処理
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void OperatorProc(CalcContextYamamoto ctx, CalcButton btn)
        {
            switch(btn)
            {
                case CalcButton.BtnPlus:
                    ctx.ResultValue += double.Parse(ctx.DisplayText);
                    break;
                case CalcButton.BtnMinus:
                    ctx.ResultValue -= double.Parse(ctx.DisplayText);
                    break;
                case CalcButton.BtnMultiple:
                    ctx.ResultValue *= double.Parse(ctx.DisplayText);
                    break;
                case CalcButton.BtnDivide:
                    ctx.ResultValue /= double.Parse(ctx.DisplayText);
                    break;
                default:
                    break;
            }

            // 計算過程を反映
            ctx.SubDisplayText += (ctx.DisplayText + GetOperatorString(btn));

            // 演算子を覚えておく
            ctx.Operator = btn;

            // 次に入力するときはディスプレイテキストを消す
            ctx.DisplayTextClearFlag = true;
        }

        /// <summary>
        /// 最終結果計算
        /// </summary>
        /// <param name="ctx"></param>
        private void EqualProc(CalcContextYamamoto ctx)
        {
            // 前回入力された演算子に応じて処理を行う
            switch(ctx.Operator)
            {
                case CalcButton.BtnPlus:
                    ctx.ResultValue += double.Parse(ctx.DisplayText);
                    break;
                case CalcButton.BtnMinus:
                    ctx.ResultValue -= double.Parse(ctx.DisplayText);
                    break;
                case CalcButton.BtnMultiple:
                    ctx.ResultValue *= double.Parse(ctx.DisplayText);
                    break;
                case CalcButton.BtnDivide:
                    ctx.ResultValue /= double.Parse(ctx.DisplayText);
                    break;
                default:
                    break;
            }

            // 結果を表示
            ctx.DisplayText = ctx.ResultValue.ToString();

            // 計算過程クリア
            ctx.SubDisplayText = "";
            ctx.ResultValue = 0;

            // 次に入力するときはディスプレイテキストを消す
            ctx.DisplayTextClearFlag = true;
        }

        /// <summary>
        /// 演算子ボタンに対応する文字を返す
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        private string GetOperatorString(CalcButton btn)
        {
            switch(btn)
            {
                case CalcButton.BtnPlus:
                    return "+";
                case CalcButton.BtnMinus:
                    return "-";
                case CalcButton.BtnMultiple:
                    return "×";
                case CalcButton.BtnDivide:
                    return "÷";
                default:
                    return "";
            }
        }
    }
}
