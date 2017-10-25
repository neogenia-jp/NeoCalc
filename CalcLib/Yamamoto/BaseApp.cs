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
            ctx.Mode = CalcSvcYamamoto.CalcContextYamamoto.AppMode.Calculator;

            // 電卓モードの初期表示テキストを設定
            ctx.SubDisplayText = "";
            ctx.DisplayText = "";
        }

        /// <summary>
        /// おみくじモードへ移行
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        internal void ToOmikujiMode(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            // おみくじモードへ変更
            ctx.Mode = CalcSvcYamamoto.CalcContextYamamoto.AppMode.Omikuji;

            // おみくじモードの初期表示テキストを設定
            ctx.SubDisplayText = "おみくじを選択してください。";
            ctx.DisplayText = "";
            for(int i = 0; i < Enum.GetNames(typeof(OmikujiApp.OmikujiKind)).Length; i++)
            {
                ctx.DisplayText += $"[{i + 1}] ";
            }
            ctx.DisplayText.TrimEnd();
        }

    }
}
