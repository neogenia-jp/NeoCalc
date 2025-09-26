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
            web.Headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 "
                + "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

            //みんかぶDJIダウ工業株30種平均のURL
            string urlText = "https://us.kabutan.jp/indexes/%5EDJI";
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

                // ^DJI のボックスを特定して price 属性を読む
                var box = doc.DocumentNode.SelectSingleNode("//div[@data-stocks--favorite-stock-code-value='^DJI']");
                if (box == null) throw new Exception("DJI block not found");

                var priceRaw = box.GetAttributeValue("data-stocks--favorite-stock-price-value", null);
                if (string.IsNullOrEmpty(priceRaw)) throw new Exception("price attribute is missing");

                // 小数点と桁区切り対応
                var price = decimal.Parse(priceRaw.Replace(",", ""));
                // 仮の日時
                var GetDowDate = DateTime.Now;

                // TODO: 時刻もスクレイピングする 現状、DateTime.Now を仮に入れている
                // ＮＹダウ平均時間
                // string getDateXPath = @"//dd[@class=""yjSb real""]";
                // ////株価取得時間
                // //string getTimeXPath = @"//dd[@class=""yjSb real""]/span";

                // var stock = doc.DocumentNode.SelectSingleNode(pricePath);
                // var time = doc.DocumentNode.SelectSingleNode(getDateXPath);
                // //var time = doc.DocumentNode.SelectSingleNode(getTimeXPath);

                // var GetDowDate = NMethod(time.InnerText);

                return new DowPrice(price, GetDowDate, DateTime.Now);
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
