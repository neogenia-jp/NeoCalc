using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Util
{
    public static class StockTimeUtil
    {
        public static string IsClosingTime(StockPrice2 sp2)
        {
            string subText = sp2.Date.ToString("yyyy.MM.dd HH:mm:ss"); ;

            //株価取得時間が15:00なら「オワリネ」を表示する
            if (sp2.Date.Hour == 15)
            {
                subText = sp2.Date.ToString("yyyy.MM.dd");
                subText += " オワリネ";
            }

            return subText;
        }
    }
}
