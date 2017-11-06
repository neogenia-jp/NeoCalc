using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    public static class SvcFactory
    {
        public static ISubContext CreateContext() => CreateService().CreateContext();

        public static ISubSvc CreateService()
        {
            ISubSvc o = null;
            return o;
        }
    }
}
