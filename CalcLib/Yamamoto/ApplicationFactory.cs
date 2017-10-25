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
        private static Calculator CreateCalculator() => _calculator == null ? _calculator = new Calculator() : _calculator;

        /// <summary>
        /// アプリ初期化
        /// </summary>
        public static void Init()
        {
            _calculator = null;
        }

        /// <summary>
        /// アプリ作成
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IApplication CreateApp(CalcSvcYamamoto.CalcContextYamamoto.AppMode mode)
        {
            switch(mode)
            {
                case CalcSvcYamamoto.CalcContextYamamoto.AppMode.Calculator:
                    return CreateCalculator();
                //case CalcContextYamamoto.AppMode.Omikuji:
                //    break;
                default:
                    return null;
            }
        }
    }
}
