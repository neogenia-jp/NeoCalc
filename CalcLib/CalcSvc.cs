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
                case CalcButton.BtnPlus:
                    ctx.Ope = btn;
                    break;
                case CalcButton.BtnMinux:
                    ctx.Ope = btn;
                    break;
                case CalcButton.BtnEqual:
                    answer = Calc(ctx.Value1, ctx.Value2, ctx.Ope.Value);
                    ctx.DisplayText = answer.ToString();
                    break;
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

            //Displayには一回一回結果が表示されている
            //ctx.DisplayText 

            //SubDisplayには履歴が表示されている
            //ctx.SubDisplayText 
        }

        public int Calc(string Value1, string Value2, CalcButton Ope)
        {
            int answer = 0;

            var val1 = int.Parse(Value1);
            var val2 = int.Parse(Value2);

            if (Ope == CalcButton.BtnPlus)
            {
                answer = val1 + val2;

            }
            else if (Ope == CalcButton.BtnMinux)
            {
                answer = val1 - val2;
            }
            return answer;
        }
    }
}
