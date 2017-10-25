using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    public interface ISubSvc
    {
        bool OnClick(ICalcContext ctx, CalcButton btn);
    }
}
