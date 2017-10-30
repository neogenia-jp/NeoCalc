using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Common.Stock
{
    public interface IStockSvc
    {
        /// <summary>
        /// Yahooファイナンスの株価をスクレイピングして株価を返す
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        decimal Scrape(int code);
    }
}
