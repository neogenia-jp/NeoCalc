using System;
namespace CalcLibCore.Tomida.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// 拡張メソッド。対象のボタンに対応するディスプレイ用文字列表現を取得します。
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static string ToDisplayString(this CalcLib.CalcButton btn)
        {
            return CalcConstants.DisplayStringDic[btn];
        }

        /// <summary>
        /// 拡張メソッド。配列の中身をシャッフルした新しい配列を返します。参照はシャローコピーです。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] Shuffle<T>(this IEnumerable<T> arrayOrigin)
        {
            T[] array = arrayOrigin.ToArray();
            // ChatGPTご提案方法
            Random _random = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return array;
        }
    }
}

