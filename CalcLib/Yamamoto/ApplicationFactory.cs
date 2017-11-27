using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    internal class ApplicationFactory
    {
        private static Calculator _calculator = null;
        private static OmikujiApp _omikujiApp = null;
        private static StockApp _stockApp = null;

        /// <summary>
        /// アプリ初期化
        /// </summary>
        public static void Init()
        {
            _calculator = null;
            _omikujiApp = null;
            _stockApp = null;
        }

        /// <summary>
        /// アプリ作成
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static BaseApp CreateApp(AppMode mode)
        {
            switch (mode)
            {
                case AppMode.Calculator:
                    return CreateCalculator();
                case AppMode.Omikuji:
                    return CreateOmikujiApp();
                case AppMode.Stock:
                    return CreateStockApp();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 電卓アプリ作成
        /// </summary>
        /// <returns></returns>
        private static Calculator CreateCalculator()
        {
            if (_calculator == null || _calculator.InputState == Calculator.State.Fin)
            {
                _calculator = new Calculator();
            }
            return _calculator;
        }

        /// <summary>
        /// おみくじアプリ作成
        /// </summary>
        /// <returns></returns>
        private static OmikujiApp CreateOmikujiApp()
        {
            if (_omikujiApp == null || _omikujiApp.InputState == OmikujiApp.State.Fin)
            {
                _omikujiApp = new OmikujiApp();
            }
            return _omikujiApp;
        }

        /// <summary>
        /// 株価取得アプリ作成
        /// </summary>
        /// <returns></returns>
        private static StockApp CreateStockApp()
        {
            if (_stockApp == null || _stockApp.InputState == StockApp.State.Fin)
            {
                _stockApp = new StockApp();
            }
            return _stockApp;
        }
    }
}
