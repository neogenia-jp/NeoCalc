using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalcLib.Maeda.Basis;

namespace CalcLib.Maeda.Util
{
    /// <summary>
    /// サービスクラスのインスタンスを動的に生成し、キャッシュも行うまあまあすごいやつ
    /// </summary>
    class SvcActivator
    {
        static Dictionary<string, IBackendSvc> cache = new Dictionary<string, IBackendSvc>();  // TODO: GCされなくなるのでWeakReferenceを使うべき

        public static IBackendSvc GetOrCreate(string className)
        {
            if (!cache.ContainsKey(className))
            {
                cache[className] = (IBackendSvc)Activator.CreateInstance(Type.GetType(className));
            }
            return cache[className];
        }
    }
}
