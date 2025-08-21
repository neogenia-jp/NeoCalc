using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcLib.Yamamoto3.Extensions;

namespace CalcLib.Yamamoto3
{
    internal class CalcSvcYamamoto3 : ICalcSvc
    {
        public virtual ICalcContext CreateContext() => new CalcContextYamamoto3();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextYamamoto3;
            if (btn.IsNumber())
            {
                ctx.State.InputNumber(ctx, btn);
            }
            else if (btn.IsOperator())
            {
                ctx.State.InputOperator(ctx, btn);
            }
            else if (btn == CalcButton.BtnEqual)
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
