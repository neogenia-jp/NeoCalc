using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcLib.Maeda.Basis
{
    /// <summary>
    /// ディスパッチャで管理可能なバックエンドサービスのインタフェース
    /// </summary>
    internal interface IBackendSvc
    {
        ICalcContext CreateContext();

        string GetExtButtonText(int num);

        /// <summary>
        /// ICalcSvcと異なり、boolを返す必要がある
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        /// <returns>デフォルトサービスに復帰する場合はfalse</returns>
        bool TryButtonClick(ICalcContext ctx, CalcButton btn);
    }

    /// <summary>
    /// バックエンドサービスのベースクラス（ジェネリックでコンテキストクラスを指定でき、よりタイプセーフになっている）
    /// </summary>
    /// <typeparam name="T">コンテキストクラス</typeparam>
    internal abstract class SvcBase<T> : IBackendSvc where T: ICalcContext
    {
        internal abstract T _CreateContext();
        public ICalcContext CreateContext() => _CreateContext();

        public abstract string GetExtButtonText(int num);

        /// <summary>
        /// 元のサービスに復帰するボタンの一覧
        /// </summary>
        protected virtual IEnumerable<CalcButton> ReturnButtons { get; } = new [] { CalcButton.BtnClear, CalcButton.BtnClearEnd };

        /// <summary>
        /// このサービスから抜けるときに呼ばれる
        /// </summary>
        protected virtual void OnExitSvc(T ctx) { }

        /// <summary>
        /// ボタンクリック時の処理（サブクラスでオーバーライドする）
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public abstract bool TryButtonClick(T ctx, CalcButton btn);

        public bool TryButtonClick(ICalcContext ctx, CalcButton btn)
        {
            var result = true;
            // 復帰ボタンであれば復帰する
            if (ReturnButtons.Any(x => x == btn))
            {
                result = false;
            }
            else
            {
                result = TryButtonClick((T)ctx, btn);
            }
            if (!result) OnExitSvc((T)ctx);
            return result;
        }

    }
}
