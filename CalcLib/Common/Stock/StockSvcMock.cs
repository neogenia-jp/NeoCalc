using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Common.Stock
{
    public class StockSvcMock : IStockSvc
    {
        public decimal Scrape(int code)
        {
            return 1000;
        }
    }
}
