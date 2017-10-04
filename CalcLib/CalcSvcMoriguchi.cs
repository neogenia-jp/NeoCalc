using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    internal class CalcSvcMoriguchi : ICalcSvc
    {
        class CalcContextMoriguchi : CalcContext
        {
            public string Value { get; set; }
            public CalcButton? Ope { get; set; }

            public CalcButton? Ope2 { get; set; }

            public string Buffer { get; set; }

            public bool Reset { get; set; }

            public string OpeName { get; set; }

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
                    ctx.Ope = btn;
                    ctx.OpeName = "＋";
                    break;
                case CalcButton.BtnMinus:
                    ctx.Ope = btn;
                    ctx.OpeName = "－";
                    break;
                case CalcButton.BtnDivide:
                    ctx.Ope = btn;
                    ctx.OpeName = "÷";
                    break;
                case CalcButton.BtnMultiple:
                    ctx.Ope = btn;
                    ctx.OpeName = "×";
                    break;

                //クリア
                case CalcButton.BtnClear:
                    ctx.Buffer = null;
                    ctx.Value = null;
                    ctx.Ope = null;
                    ctx.Ope2 = null;
                    ctx.DisplayText = "0";
                    ctx.SubDisplayText = null;
                    break;
                case CalcButton.BtnClearEnd:
                    break;
                case CalcButton.BtnBS:
                    //TODO
                    break;

                //計算
                case CalcButton.BtnEqual:
                    if (!string.IsNullOrEmpty(ctx.Buffer) && !string.IsNullOrEmpty(ctx.Value) && ctx.Ope2 != null)
                    {
                        ctx.Buffer = Calc(ctx.Value, ctx.Buffer, ctx.Ope2.Value).ToString();
                        ctx.Value = null;
                        ctx.Ope = null;
                        ctx.Ope2 = null;
                        ctx.Reset = true;
                        ctx.SubDisplayText = null;
                    }
                    break;

                //入力数値取得
                default:
                    if (ctx.Reset)
                    {
                        ctx.Buffer = null;
                        ctx.DisplayText = null;
                        ctx.Reset = false;
                    }
                    ctx.Buffer += (int)btn;
                    break;
            }

            //値の処理
            if (ctx.Ope != null)
            {
                ctx.Ope2 = ctx.Ope;
                ctx.SubDisplayText += ctx.Buffer;


                if (string.IsNullOrEmpty(ctx.Value))
                {
                    ctx.Value = ctx.Buffer;
                    ctx.Buffer = null;
                    ctx.Ope = null;
                    ctx.SubDisplayText += ctx.OpeName;
                    ctx.OpeName = null;
                }
                else if (!string.IsNullOrEmpty(ctx.Buffer))
                {
                    ctx.Value = Calc(ctx.Value, ctx.Buffer, ctx.Ope2.Value).ToString();
                    ctx.Ope = null;
                    ctx.Buffer = null;
                    ctx.Reset = true;
                    ctx.SubDisplayText += ctx.OpeName;
                    ctx.OpeName = null;
                }
            }

            //表示
            ctx.DisplayText = ctx.Buffer == null ? ctx.Value : ctx.Buffer;
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
