﻿using System;
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
            // 面倒なので直接指定
            return new CalcLibCore.Tomida2.CalcSvcTomida2();
            /*
            // 環境変数「CalcSvcClass」よりクラス名を決定する
            var clsName = Environment.GetEnvironmentVariable("CalcSvcClass");
            if (string.IsNullOrWhiteSpace(clsName))
            {
                return new CalcSvc();
            }

            var obj = Activator.CreateInstance(Assembly.GetExecutingAssembly().GetTypes().First(x=>x.Name == clsName));
            return obj as ICalcSvc;
            */
        }
    }

}
