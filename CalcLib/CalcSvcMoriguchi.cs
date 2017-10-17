using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    internal class CalcSvcMoriguchi : ICalcSvcEx
    {
        class OpeNameHelper
        {
            static readonly Dictionary<CalcButton, string> OpeTextTable = new Dictionary<CalcButton, string>
            {
              { CalcButton.BtnPlus, "+" },
              { CalcButton.BtnMinus, "-" },
              { CalcButton.BtnMultiple, "×" },
              { CalcButton.BtnDivide, "÷"},
            };
            public static string Get(CalcButton? opeButton) => opeButton.HasValue ? OpeTextTable[opeButton.Value] : "";
        }

        class CalcContextMoriguchi : ICalcContext
        {
            /// <summary>
            /// 左辺の値
            /// </summary>
            public string Value { get; set; }
            
            /// <summary>
            /// 計算対象とする演算子
            /// </summary>
            public CalcButton? Operation { get; set; }

            /// <summary>
            /// 入力中の値（Valueがセットされている時は右辺となる）
            /// </summary>
            public string Buffer { get; set; }

            public bool Reset { get; set; }

            public string DisplayText => Buffer == null ? Value : Buffer;

            /// <summary>
            /// サブディスプレイに表示する文字列
            /// </summary>
            public virtual string SubDisplayText => Operation == null ? "" : Value + OpeNameHelper.Get(Operation);
        }

        public virtual ICalcContext CreateContext() => new CalcContextMoriguchi();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextMoriguchi;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            switch (btn)
            {
                //演算子
                case CalcButton.BtnPlus:
                case CalcButton.BtnMinus:
                case CalcButton.BtnDivide:
                case CalcButton.BtnMultiple:
                    OnOpeButtonClick(ctx, btn);   // 演算子ボタン押下時の処理
                    break;

                //クリア
                case CalcButton.BtnClear:
                    ctx.Buffer = null;
                    ctx.Value = null;
                    ctx.Operation = null;
                    //ctx.SubDisplayText = null;
                    break;
                case CalcButton.BtnClearEnd:
                    break;
                case CalcButton.BtnBS:
                    //TODO
                    break;

                //計算
                case CalcButton.BtnEqual:
                    if (!string.IsNullOrEmpty(ctx.Buffer) && !string.IsNullOrEmpty(ctx.Value) && ctx.Operation != null)
                    {
                        ExecCalcuration(ctx, ctx.Operation.Value);
                        ctx.Operation = null;
                        //ctx.SubDisplayText = null;
                    }
                    break;

                //入力数値取得
                default:
                    if (ctx.Reset)
                    {
                        ctx.Buffer = null;
                        ctx.Reset = false;
                    }
                    ctx.Buffer += (int)btn;
                    break;
            }
        }

        /// <summary>
        /// 演算子ボタン押下時の処理
        /// </summary>
        /// <param name="ctx"></param>
        private void OnOpeButtonClick(CalcContextMoriguchi ctx, CalcButton btn)
        {
            ctx.Operation = btn;
            //ctx.SubDisplayText += ctx.Buffer;

            if (string.IsNullOrEmpty(ctx.Value))
            {
                // 左辺が未入力の時、Bufferの値を左辺とする
                ctx.Value = ctx.Buffer;
                ctx.Buffer = null;
                //ctx.SubDisplayText += ctx.Buffer + OpeNameHelper.Get(btn);
            }
            else if (!string.IsNullOrEmpty(ctx.Buffer))
            {
                // 左辺が入力済みで、Bufferが入力済みの時、計算処理を実行する
                var x = ExecCalcuration(ctx, btn);

                // 続けて計算できるよう実行結果を左辺にセットする。
                ctx.Value = x;
                ctx.Buffer = null;
                //ctx.SubDisplayText += OpeNameHelper.Get(btn);
            }
        }

        /// <summary>
        /// 計算処理を実行します
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private string ExecCalcuration(CalcContextMoriguchi ctx, CalcButton operation)
        {
            string x;
            {
                x = Calc(ctx.Value, ctx.Buffer, operation).ToString();
                ctx.Reset = true;
                ctx.Buffer = x;
                ctx.Value = null;
            }

            return x;
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="Value1"></param>
        /// <param name="Value2"></param>
        /// <param name="Ope"></param>
        /// <returns></returns>
        public double Calc(string Value, string Buffer, CalcButton Ope)
        {
            double answer = 0;

            var val1 = double.Parse(Value);
            var val2 = double.Parse(Buffer);

            if (Ope == CalcButton.BtnPlus)
            {
                answer = val1 + val2;
            }
            else if (Ope == CalcButton.BtnMinus)
            {
                answer = val1 - val2;
            }
            else if (Ope == CalcButton.BtnMultiple)
            {
                answer = val1 * val2;
            }
            else if (Ope == CalcButton.BtnDivide)
            {
                answer = val1 / val2;
            }
            return answer;
        }
    }
}
