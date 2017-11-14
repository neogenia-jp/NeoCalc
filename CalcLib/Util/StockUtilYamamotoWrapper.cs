using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using AngleSharp.Parser.Html;

namespace CalcLib.Util
{
    public class StockUtilYamamotoWrapper
    {
        private static StockUtilYamamotoWrapper _wrapper;
        private StockPrice _sp;
        private ApplicationException _ex;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private StockUtilYamamotoWrapper() { }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        public static StockUtilYamamotoWrapper GetInstance()
        {
            if (_wrapper == null) _wrapper = new StockUtilYamamotoWrapper();
            return _wrapper;
        }

        /// <summary>
        /// 返してもらう株価データを登録しておける
        /// (テスト用の裏口メソッド)
        /// </summary>
        /// <param name="src"></param>
        internal void RegistStockData(StockPrice src) => _sp = new StockPrice(src.Code, src.Price, src.Date);

        /// <summary>
        /// 返してもらう例外を登録しておける
        /// (テスト用の裏口メソッド)
        /// </summary>
        /// <param name="ex"></param>
        internal void RegistException(Exception ex) => _ex = new ApplicationException("テスト例外", ex);

        /// <summary>
        /// 株価を取得する
        /// </summary>
        /// <param name="code">証券コード4桁</param>
        /// <returns>株価情報</returns>
        public StockPrice GetStockPrice(string code)
        {
            // 例外が設定されている場合は例外を返す
            if (_ex != null) throw _ex;

            return _sp == null ? StockUtilYamamoto.GetStockPrice(code) : _sp;
        }
    }
}
