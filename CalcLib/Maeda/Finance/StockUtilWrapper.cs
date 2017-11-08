using CalcLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Maeda.Finance
{
    /// <summary>
    /// StockUtil をラップするためのインタフェース。
    /// 本来であれば StockUtil 自体がインタフェースを定義するべきであるが、
    /// そうなっていないコードを対象に、それをラップせざるを得ないケースと想定しています。
    /// </summary>
    public interface IStockUtil
    {
        /// <summary>
        /// 株価取得
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        StockPrice GetStockPrice(string code);
    }

    /// <summary>
    /// Utilクラスの実体取得を隠蔽するためのファクトリクラス
    /// Activatorパターンによって実行時に動的に型を指定できる。
    /// </summary>
    public static class UtilActivator
    {
        /// <summary>
        /// 型を指定してインスタンス取得を行う
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() => Imple.UtilActivatorImple.Get<T>();
    }

    /// <summary>
    /// このnamespaceは内部実装クラスを隠蔽するためにわざわざ通常のnamespaceと階層を分けている。
    /// プロダクトコードから using しない限り、この中のクラスは InteliSence で表示されないはず。
    /// </summary>
    namespace Imple
    {
        /// <summary>
        /// IStockUtil を実装した StockUtil の純粋なラッパー
        /// </summary>
        internal class StockUtilWrapper : IStockUtil {
            public StockPrice GetStockPrice(string code) => StockUtil.GetStockPrice(code);
        }

        /// <summary>
        /// UtilActivator の実体
        /// </summary>
        internal static class UtilActivatorImple
        {
            private static IDictionary<Type, Type> _mapping { get; } = new Dictionary<Type, Type>();

            /// <summary>
            /// インタフェース型に対してインスタンス生成させる実体クラスを登録する
            /// </summary>
            /// <typeparam name="I">インタフェース型</typeparam>
            /// <typeparam name="R">RはIを実装した型でないといけない</typeparam>
            internal static void Registor<I, R>() where R : I
            {
                _mapping.Add(typeof(I), typeof(R));
            }

            public static T Get<T>()
            {
                /// 登録されていればその型を生成して返す
                if (_mapping.ContainsKey(typeof(T)))
                {
                    return (T)Activator.CreateInstance(_mapping[typeof(T)]);
                }

                // 登録されていなければ、既知のインタフェースに対してはデフォルトの型を返す
                if (typeof(T) == typeof(IStockUtil))
                {
                    return (T)(object)new StockUtilWrapper();
                }

                // 未知の型に対しては default 演算子で対応する。（例外を投げるべき？）
                return default(T);
            }
        }
    }
}
