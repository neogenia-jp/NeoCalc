using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcLib.Maeda.Basis
{
    /// <summary>
    /// サービスインスタンスにIDを付けて管理するための入れ物。全然すごくない。
    /// </summary>
    internal class SvcHolder
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// サービスインスタンス
        /// </summary>
        public IBackendSvc Svc { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="svc"></param>
        public SvcHolder(int id, IBackendSvc svc) { Id = id; Svc = svc; }

        /// <summary>
        /// 複製を作る。Duplicationの略。
        /// </summary>
        /// <returns></returns>
        public SvcHolder Dup() => new SvcHolder(Id, Svc);
    }
}
