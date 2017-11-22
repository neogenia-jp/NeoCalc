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
        
        internal static void _Uraguchi(int v)
        {
            // v をどこかに保存する
            sPrice = new StockPrice2("1301",v,DateTime.Now,new DateTime(2017,11,22,11,22,33));
        }

        //株価取得時例外の設定
        internal static void _UraguchiExeption(Exception e)
        {
            ex = e;
        }

        //株価取得日時の設定
        internal static void _UraguchiDate(DateTime date)
        {
            sPrice = new StockPrice2("1301", 1000, date, new DateTime(2017, 11, 22, 11, 22, 33));
        }

    }
}