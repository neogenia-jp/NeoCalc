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
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");
        }
    }
}
