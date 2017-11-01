using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcLib.Maeda.Util
{
    public static class EnumerableExt
    {
        /// <summary>
        /// Shuffle a list using Fisher-Yates Algorythm.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list0"></param>
        /// <returns></returns>
        public static IList<T> Shuffle<T>(this IEnumerable<T> list0)
        {
            var list = list0.ToList();
            var r = new Random();
            var n = list.Count;
            while (n > 1)
            {
                var k = r.Next(--n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
