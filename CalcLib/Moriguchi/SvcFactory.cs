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
            var No = CalcSvcMoriguchi.SvcNo;
            object o = new CalcClass();

            switch (No)
            {
                case 21:
                    o = new OmikujiClass();
                    break;
                default:
                    o = new CalcClass();
                    break;
            }

            return o as ISubSvc;
        }
    }
}
