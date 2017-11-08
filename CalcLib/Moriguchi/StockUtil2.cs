using CalcLib.Util;
using System;

namespace CalcLib.Moriguchi
{
    public class StockUtil2
    {
        static StockPrice sPrice;

        public static StockPrice GetStockPrice(string code)
        {
            // 裏口からの設定があれば、その v を返す
            if (sPrice != null)
            {
                return sPrice;
            }
            return StockUtil.GetStockPrice(code);
        }


        internal static void _Uraguchi(int v)
        {
            // v をどこかに保存する
            sPrice = new StockPrice("1301",v,DateTime.Now);
        }
    }
}