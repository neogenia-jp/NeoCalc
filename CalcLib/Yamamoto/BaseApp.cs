using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    public abstract class BaseApp
    {
        internal AppMode NextMode { get; set; } = AppMode.None;

        /// <summary>
        /// 電卓モードへ移行
        /// </summary>
        internal void ToCaliculatorMode() => NextMode = AppMode.Calculator;

        /// <summary>
        /// おみくじモードへ移行
        /// </summary>
        internal void ToOmikujiMode() => NextMode = AppMode.Omikuji;

        public abstract void Run(ICalcContext ctx0, CalcButton btn);
    }
}
