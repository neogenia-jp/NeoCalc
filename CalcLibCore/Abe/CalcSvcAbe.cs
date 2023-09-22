using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Abe
{
    internal class CalcSvcAbe : ICalcSvc
    {
        class CalcContextAbe : CalcContext
        {
        }

        public virtual ICalcContext CreateContext() => new CalcContextAbe();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContext;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            ctx.DisplayText = "Maeda";
        }
    }
}
