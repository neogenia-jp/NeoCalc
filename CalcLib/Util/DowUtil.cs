using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Util
{
    public class DowPrice
    {
        public decimal Price { get; set; }
        public DateTime Date { get; }
        public DateTime PriceGetDate { get; }
        public DowPrice(decimal p, DateTime d, DateTime dt) { Price = p ;Date = d; PriceGetDate = dt; }
    }

    public static class DowUtil
    {
        // <summary>
        /// ＮＹダウ平均を取得する
        /// </summary>
        /// <param name="code">証券コード4桁</param>
        /// <returns>株価情報</returns>
        public static DowPrice GetDowPrice()
        {
            //HTMLのコードを文書として保存
            var doc = new HtmlAgilityPack.HtmlDocument();
            var web = new System.Net.WebClient();
            web.Encoding = Encoding.UTF8;

            //Yahooファイナンスの株式ページURL
            string urlText = "https://stocks.finance.yahoo.co.jp/stocks/detail/?code=^DJI";
            var html = "";

            try
            {
                html = web.DownloadString(urlText);
            }
            catch (Exception e)
            {
                throw new ApplicationException("エラーが発生しました", e)
                {
                    Data = { { "エラー種別", "NETWORK ERROR" } }
                };
            }

            try
            {
                //webを通してHTMLのコード取得
                doc.LoadHtml(html);

                //ＮＹダウ平均のXPath
                string pricePath = @"//td[@class=""stoksPrice""]";
                //ＮＹダウ平均時間
                string getDateXPath = @"//dd[@class=""yjSb real""]";
                ////株価取得時間
                //string getTimeXPath = @"//dd[@class=""yjSb real""]/span";

                var stock = doc.DocumentNode.SelectSingleNode(pricePath);
                var time = doc.DocumentNode.SelectSingleNode(getDateXPath);
                //var time = doc.DocumentNode.SelectSingleNode(getTimeXPath);
              
                var GetDowDate = NMethod(time.InnerText);
                
                return new DowPrice(decimal.Parse(stock.InnerText), GetDowDate, DateTime.Now);
            }
            catch (Exception e)
            {
                throw new ApplicationException("エラーが発生しました", e)
                {
                    Data = { { "エラー種別", "SCRAPING ERROR" } }
                };
            }
        }

        /// <summary>
        /// 取得した時刻からDatetimeへ変換 
        /// 06:45（現地時刻：16:45）→　ex. 2017.05.28 16:45
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="Time"></param>
        private static DateTime NMethod(string time)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var date = DateTime.Now.Day;
            var hour = time.Substring(time.IndexOf('刻') + 2,2);
            var minute = time.Substring(time.IndexOf('刻') + 5, 2);

            return new DateTime(year, month, date, int.Parse(hour), int.Parse(minute), 0);
        }
    }
}
