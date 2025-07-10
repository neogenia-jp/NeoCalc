using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto3
{
    internal class CalcSvcYamamoto3 : ICalcSvc
    {
        public virtual ICalcContext CreateContext() => new CalcContextYamamoto3();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextYamamoto3;
            if(btn == CalcButton.Btn0 || btn == CalcButton.Btn1 || btn == CalcButton.Btn2 ||
               btn == CalcButton.Btn3 || btn == CalcButton.Btn4 || btn == CalcButton.Btn5 ||
               btn == CalcButton.Btn6 || btn == CalcButton.Btn7 || btn == CalcButton.Btn8 ||
               btn == CalcButton.Btn9)
            {
                ctx.State.InputNumber(ctx, btn);
            }
            else if(btn == CalcButton.BtnPlus || btn == CalcButton.BtnMinus ||
                    btn == CalcButton.BtnDivide || btn == CalcButton.BtnMultiple)
            {
                ctx.State.InputOperator(ctx, btn);
            }
            else if(btn == CalcButton.BtnEqual)
            {
                ctx.State.InputEqual(ctx, btn);
            }
            else
            {
                // その他のボタンは何もしない
            }
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");
        }
    }
}
