using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Util
{
    public static class StockTimeUtil
    {
        public static string CheckDate(DateTime date)
        {
            string subText = date.ToString("yyyy.MM.dd HH:mm:ss"); ;

            //TODO:実際の取引時間、平日9:00～11:30・12:30～15:00までを考慮する
            //上記時間以外や土・日・祝日、年末年始は休み

            //株価取得時間が15:00以降9:00未満なら「オワリネ」を表示する
            if (date.Hour < 9 || date.Hour >= 15)
            {
                subText = date.ToString("yyyy.MM.dd");
                subText += " オワリネ";
            }

            return subText;
        }
    }
}
