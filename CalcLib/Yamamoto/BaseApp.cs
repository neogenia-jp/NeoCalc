using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    public class BaseApp
    {
        /// <summary>
        /// 電卓モードへ移行
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        internal void ToCaliculatorMode(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            // 電卓モードへ変更
            ctx.BeforeMode = ctx.Mode;
            ctx.Mode = CalcSvcYamamoto.CalcContextYamamoto.AppMode.Calculator;
        }

        /// <summary>
        /// おみくじモードへ移行
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        internal void ToOmikujiMode(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            // おみくじモードへ変更
            ctx.BeforeMode = ctx.Mode;
            ctx.Mode = CalcSvcYamamoto.CalcContextYamamoto.AppMode.Omikuji;
        }

    }
}
