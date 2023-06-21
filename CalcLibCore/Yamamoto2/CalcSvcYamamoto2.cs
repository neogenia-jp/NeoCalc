using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto2
{
    internal class CalcSvcYamamoto2 : ICalcSvc
    {
        class CalcContextYamamoto2 : CalcContext
        {
        }

        public virtual ICalcContext CreateContext() => new CalcContextYamamoto2();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContext;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            ctx.DisplayText = "Yamamoto2";
        }
    }
}
