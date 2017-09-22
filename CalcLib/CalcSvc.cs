using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    internal class CalcSvc : ICalcSvc
    {
        public virtual ICalcContext CreateContext() => new CalcContext();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContext;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            var answer = 0;

            switch (btn)
            {
                //演算子
                case CalcButton.BtnPlus:
                    ctx.Ope = btn;
                    break;
                case CalcButton.BtnMinus:
                    ctx.Ope = btn;
                    break;
                case CalcButton.BtnDivide:
                    ctx.Ope = btn;
                    break;
                case CalcButton.BtnMultiple:
                    ctx.Ope = btn;
                    break;

                //計算
                case CalcButton.BtnEqual:
                    answer = Calc(ctx.Value1, ctx.Value2, ctx.Ope.Value);
                    ctx.DisplayText = answer.ToString();
                    break;

                //クリア
                case CalcButton.BtnClear:

                    break;
                case CalcButton.BtnClearEnd:

                    break;
                case CalcButton.BtnBS:

                    break;

                //値入力
                default:
                    if (ctx.Ope == null)
                    {
                        ctx.Value1 += (int)btn;
                        ctx.DisplayText = ctx.Value1;
                    }
                    else
                    {
                        ctx.Value2 += (int)btn;
                        ctx.DisplayText = ctx.Value2;
                    }
                    break;
            }


            //出力

            //Displayには一回一回結果が表示されている
            //ctx.DisplayText 

            //SubDisplayには履歴が表示されている
            //ctx.SubDisplayText 
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="Value1"></param>
        /// <param name="Value2"></param>
        /// <param name="Ope"></param>
        /// <returns></returns>
        public int Calc(string Value1, string Value2, CalcButton Ope)
        {
            int answer = 0;

            var val1 = int.Parse(Value1);
            var val2 = int.Parse(Value2);

            if (Ope == CalcButton.BtnPlus)
            {
                answer = val1 + val2;
            }
            else if (Ope == CalcButton.BtnMinus)
            {
                answer = val1 - val2;
            }
            else if (Ope == CalcButton.BtnDivide)
            {
                answer = val1 * val2;
            }
            else if (Ope == CalcButton.BtnMultiple)
            {
                answer = val1 / val2;
            }
            return answer;
        }
    }
}
