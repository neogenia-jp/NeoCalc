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
            //var ClassName = CalcSvcMoriguchi.SvcName[CalcSvcMoriguchi.SvcNo];

            //object o = Activator.CreateInstance(Type.GetType(ClassName));
            object o = new CalcClass();
            return o as ISubSvc;
        }
    }
}
