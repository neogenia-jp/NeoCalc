using CalcLib.Util;
using System;

namespace CalcLib.Moriguchi
{
    public class StockUraguchiUtil
    {
        static StockPrice2 sPrice;

        static Exception ex;

        public static StockPrice2 GetStockPrice(string code)
        {
            //裏口からの例外設定があれば、その例外を返す
            if (ex != null)
            {
                var kari = ex;
                ex = null;
                throw kari;
            }
            // 裏口からの設定があれば、その v を返す
            else if (sPrice != null)
            {
                var kari = sPrice;
                sPrice = null;
                return kari;
            }

            return StockUtil2.GetStockPrice(code);
        }
        
        internal static void _Uraguchi(int v, DateTime stockTime, DateTime getStockTime)
        {
            // v をどこかに保存する
            sPrice = new StockPrice2("1301",v, stockTime, getStockTime);
        }

        //株価取得時例外の設定
        internal static void _UraguchiExeption(Exception e)
        {
            ex = e;
        }
    }
}