using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CalcLib.Common.Stock
{
    public class StockSvcFactory
    {
        public static IStockSvc Create()
        {
            if(ConfigurationManager.AppSettings["TestMode"] != "true")
            {
                return new StockSvc();
            }
            return new StockSvcMock();
        }
    }
}
