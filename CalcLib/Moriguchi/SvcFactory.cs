using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    public static class SvcFactory
    {
        public static ISubContext CreateContext() => CreateService().CreateContext();

        public static ISubSvc CreateService()
        {




            //// 環境変数「CalcSvcClass」よりクラス名を決定する
            //var clsName = Environment.GetEnvironmentVariable("CalcSvcClass");
            //if (string.IsNullOrWhiteSpace(clsName))
            //{
            //    return new CalcSvc();
            //}

            ICalcSvc obj = null;
            return obj as ICalcSvc;
        }


    }
}
