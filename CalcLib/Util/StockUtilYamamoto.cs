using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using AngleSharp.Parser.Html;

namespace CalcLib.Util
{
    public static class StockUtilYamamoto
    {
        /// <summary>
        /// 日経平均株価
        /// </summary>
        public const string N225_CODE = "998407";

        /// <summary>
        /// NYダウ
        /// </summary>
        public const string NY_DOW_CODE = "^DJI";

        /// <summary>
        /// 株価を取得する
        /// </summary>
        /// <param name="code">証券コード4桁</param>
        /// <returns>株価情報</returns>
        public static StockPrice GetStockPrice(string code)
        {
            var url = $"https://stocks.finance.yahoo.co.jp/stocks/detail/?code={code}";
            string html;
            decimal price;
            DateTime date;

            try
            {
                html = GetHtmlDocument(url);
                price = GetPrice(html);
                date = GetDateTime(html);
            }
            catch(Exception ex)
            {
                throw new ApplicationException("株価取得時に異常が発生しました。", ex);
            }

            return new StockPrice(code, price, date);
        }

        /// <summary>
        /// YahooファイナンスのHTMLから株価を取得する
        /// </summary>
        /// <param name="html">YahooファイナンスのHTML文字列</param>
        /// <returns>株価</returns>
        private static decimal GetPrice(string html)
        {
            decimal result;
            string price_str = GetInnerText(html, "td[class=stoksPrice]");
            // TryParseに失敗した場合は0になる
            decimal.TryParse(price_str, out result);
            return result;
        }

        /// <summary>
        /// YahooファイナンスのHTMLからスクレイピングした日時を取得
        /// </summary>
        /// <param name="html">YahooファイナンスのHTML文字列</param>
        /// <returns>日時</returns>
        private static DateTime GetDateTime(string html)
        {
            // 書式 : "MM月 dd日 HH:MM"
            // 月日時分しか書いていないため、年は端末の年、秒は0秒としておく
            var date_separator = new char[] { '月', '日', ':'};
            var date_str = GetInnerText(html, "#globalDate strong");
            var year = DateTime.Now.Year;
            var date_split = date_str.Split(date_separator);

            return new DateTime( year,
                                int.Parse(date_split[0].Trim()),
                                int.Parse(date_split[1].Trim()),
                                int.Parse(date_split[2].Trim()),
                                int.Parse(date_split[3].Trim()),
                                0);
        }

        /// <summary>
        /// 指定したURLのHTMLを取得する
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlDocument(string url, Encoding e = null)
        {
            if (e == null) e = Encoding.UTF8;
            string html = "";
            using (var client = new WebClient())
            {
                client.Encoding = e;
                try
                {
                    html = client.DownloadString(url);
                }
                catch(Exception ex)
                {
                    throw new WebException($"HTML文書の取得に失敗しました。指定URL[{url}]", ex);
                }
            }
            return html;
        }

        /// <summary>
        /// HTMLから指定されたセレクタの値を取得する
        /// </summary>
        /// <param name="html"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static string GetInnerText(string html, string selector)
        {
            var parser = new HtmlParser().Parse(html);
            return parser.QuerySelector(selector)?.TextContent;
        }
    }
}
