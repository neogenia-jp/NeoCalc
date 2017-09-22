using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    public static class Factory
    {
        public static ICalcContext CreateContext() => CreateService().CreateContext();

        public static ICalcSvc CreateService()
        {
            // 環境変数「CalcSvcClass」よりクラス名を決定する
            var clsName = Environment.GetEnvironmentVariable("CalcSvcClass");
        //    if (string.IsNullOrWhiteSpace(clsName))
            {
                return new CalcSvc();
            }

            var obj = Activator.CreateInstance(Assembly.GetExecutingAssembly().GetType($"CalcLib.{clsName}"));
            return obj as ICalcSvc;
        }
    }

}
