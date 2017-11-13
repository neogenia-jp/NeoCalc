using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Util
{
    /// <summary>
    /// 株価を表すクラス
    /// </summary>
    public class StockPrice
    {
        public string Code { get; }
        public decimal Price { get; }
        public DateTime Date { get; }

        public StockPrice(string c, decimal p, DateTime d) { Code = c; Price = p;  Date = d; }
    }

    public static class StockUtil
    {
        /// <summary>
        /// 株価を取得する
        /// </summary>
        /// <param name="code">証券コード4桁</param>
        /// <returns>株価情報</returns>
        public static StockPrice GetStockPrice(string code)
        {
            // FIXME
            return new StockPrice(code, 1000m, new DateTime(2017, 11, 1));
        }
    }
}
