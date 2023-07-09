using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Takao
{
    internal class CalcSvcTakao : ICalcSvc
    {
        CalclatorSvc svc = new();

        public virtual ICalcContext CreateContext() => new CalcContextTakao();

        public virtual void OnButtonClick(ICalcContext ctx, CalcButton btn) => svc.Exec(ctx, btn); 
    }
}
