using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CalcLib.Util
{
    /// <summary>
    /// StockUtilのラッパークラス（シングルトン）
    /// </summary>
    public class StockUtilWrapper
    {
        private static StockUtilWrapper _wrapper;
        private StockPrice _sp;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private StockUtilWrapper() { }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        public static StockUtilWrapper GetInstance()
        {
            if (_wrapper == null) _wrapper = new StockUtilWrapper();
            return _wrapper;
        }

        /// <summary>
        /// 返してもらう株価データを登録しておける
        /// </summary>
        /// <param name="src"></param>
        public void Regist(StockPrice src) => _sp = new StockPrice(src.Code, src.Price, src.Date);

        /// <summary>
        /// 株価を取得する
        /// </summary>
        /// <param name="code">証券コード4桁</param>
        /// <returns>株価情報</returns>
        public StockPrice GetStockPrice(string code) => _sp == null ? StockUtil.GetStockPrice(code) : _sp;
    }
}
