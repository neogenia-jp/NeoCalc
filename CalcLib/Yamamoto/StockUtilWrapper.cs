using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CalcLib.Util;

namespace CalcLib.Yamamoto
{
    public static class StockUtilWrapper
    {
        /// <summary>
        /// 株価を取得する
        /// </summary>
        /// <param name="code">証券コード4桁</param>
        /// <returns>株価情報</returns>
        public static StockPrice GetStockPrice(string code)
        {
            // テストモード以外のときは本番機能を使う
            if (ConfigurationManager.AppSettings["TestMode"] != "true")
            {
                return StockUtil.GetStockPrice(code);
            }
            return new StockPrice(code, 1000m, new DateTime(2017, 11, 1));
        }
    }
}
