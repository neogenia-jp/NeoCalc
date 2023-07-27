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
        public virtual ICalcContext CreateContext() => new CalcContextYamamoto2();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextYamamoto2;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            var executor = Executors.ExecutorFactory.Create(ctx, btn);
            executor.Execute();

            System.Diagnostics.Debug.WriteLine(ctx.ToString());
        }
    }
}
