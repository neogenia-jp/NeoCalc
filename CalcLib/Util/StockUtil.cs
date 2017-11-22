using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

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
            //HTMLのコードを文書として保存
            var doc = new HtmlAgilityPack.HtmlDocument();
            var web = new System.Net.WebClient();

            //Yahooファイナンスの株式ページURL
            string URLText = "https://stocks.finance.yahoo.co.jp/stocks/detail/?code=";
            //証券コードをURLに追加
            URLText += code;

            var html = "";

            try
            {
                html = web.DownloadString(URLText);
            }
            catch (Exception e)
            {
                throw new ApplicationException("エラーが発生しました", e) {
                    Data = { { "エラー種別", "ネットワークエラー" } }
                };
            }

            try
            {
                //webを通してHTMLのコード取得
                doc.LoadHtml(html);

                //株価を示す部分をXPathで指定
                string xPath = @"//td[@class=""stoksPrice""]";

                var stock = doc.DocumentNode.SelectSingleNode(xPath);

                return new StockPrice(code, decimal.Parse(stock.InnerText), DateTime.Now);
            }
            catch (Exception e)
            {
                throw new ApplicationException("エラーが発生しました", e) {
                    Data = { { "エラー種別", "スクレイピングエラー" } }
                };
            }
        }
    }
}
