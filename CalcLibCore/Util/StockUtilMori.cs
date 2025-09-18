using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CalcLib.Util
{
    public static class StockUtilMori
    {
        /// <summary>
        /// 株価を取得する
        /// </summary>
        /// <param name="code">証券コード4桁</param>
        /// <returns>株価情報</returns>
        public static StockPrice GetStockPrice(string code)
        {
            //HTMLのコードを文書として保存
            var doc = new HtmlAgilityPack.HtmlDocument();
            var web = new System.Net.WebClient();
            web.Headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 "
            + "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

            // https://minkabu.jp/stock/100000018 日経平均
            // https://us.kabutan.jp/indexes/%5EDJI ＮＹダウ
            // みんかぶの株式ページURL
            string URLText = "https://minkabu.jp/stock/100000018";
            //証券コードをURLに追加
            // みんかぶの株式ページURL
            URLText = $"https://minkabu.jp/stock/{code}";

            var html = "";

            try
            {
                html = web.DownloadString(URLText);
            }
            catch (Exception e)
            {
                throw new ApplicationException("エラーが発生しました", e) {
                    Data = { { "エラー種別", "NETWORK ERROR" } }
                };
            }

            try
            {
                //webを通してHTMLのコード取得
                doc.LoadHtml(html);

                //株価を示す部分をXPathで指定
                string xPath = @"//div[@class=""stock_price""]";

                var stock = doc.DocumentNode.SelectSingleNode(xPath);
                string stockText = stock.InnerText;
                string cleaned = Regex.Replace(stockText, @"[^\d.,]", "");

                return new StockPrice(code, decimal.Parse(cleaned), DateTime.Now);
            }
            catch (Exception e)
            {
                throw new ApplicationException("エラーが発生しました", e) {
                    Data = { { "エラー種別", "SCRAPING ERROR" } }
                };
            }
        }

        /// <summary>
        /// ＮＹダウ平均を取得する
        /// </summary>
        /// <returns>株価情報</returns>
        public static DowPrice GetDowPrice()
        {
            // スクレイピングせず仮の値を返す
            // return new DowPrice(decimal.Parse("45,757.90"),  DateTime.Now, DateTime.Now);
            
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
    }
}
