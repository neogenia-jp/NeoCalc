using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    internal static class DecimalExt
    {
        /// <summary>
        /// カンマ付き文字列へ変換
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        internal static string ToCommaString(this decimal num)
        {
            // TODO ToStringかstring.Format書式指定でなんとかならないだろうか・・・
            var parts = num.ToString().Split(new char[] { '.' });
            string integerPart = decimal.Parse(parts[0]).ToString("#,0");
            if(parts.Length == 1)
            {
                return integerPart;
            }

            return integerPart + "." + parts[1];
        }

        /// <summary>
        /// 後方の0をカット
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        internal static decimal CutTrailingZero(this decimal num)
        {
            if(num.ToString().IndexOf('.') < 1)
            {
                return num;
            }

            var numString = num.ToString();

            // 末尾の0をカット
            numString = numString.TrimEnd('0');
            if(numString.Last() == '.')
            {
                // 最後が"."の場合はそれもカット
                numString = numString.TrimEnd('.');
            }

            return decimal.Parse(numString);
        }

        /// <summary>
        /// ディスプレイテキスト用の文字列に変換する
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        internal static string ToDisplayText(this decimal num)
        {
            return num.CutTrailingZero().ToCommaString();
        }
    }
}
