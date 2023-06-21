using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Tomida
{
    internal class CalcSvcTomida : ICalcSvc
    {
        class CalcContextTomida : CalcContext
        {
        }

        public virtual ICalcContext CreateContext() => new CalcContextTomida();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContext;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            ctx.DisplayText = "Maeda";
        }
    }
}
