using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CalcLib.Util
{
    public class StockUtilFactory
    {
        private static StockUtilWrapper _su;

        public static StockUtilWrapper Create()
        {
            if (_su == null) _su = new StockUtilWrapper();
            return _su;
        }
    }
}
