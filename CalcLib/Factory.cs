using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    public static class Factory
    {
        public static ICalcContext CreateContext()
        {
            return new CalcContext();
        }

        public static ICalcSvc CreateService()
        {
            return new CalcSvc();
        }
    }

}
