using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalcLib.Maeda.Dispatcher;

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
        /// サービス切り替わったときに呼ばれる
        /// </summary>
        /// <param name="svcSwichedEventArg"></param>
        void OnEnter(ICalcContext ctx, SvcSwichedEventArg svcSwichedEventArg);

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

        /// <summary>
        /// サービス切り替え時のイベント
        /// （必要に応じてサブクラスでオーバーライドしてください）
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="svcSwichedEventArg"></param>
        public virtual void OnEnter(T ctx, SvcSwichedEventArg svcSwichedEventArg) { }

        public void OnEnter(ICalcContext ctx, SvcSwichedEventArg svcSwichedEventArg) => OnEnter((T)ctx, svcSwichedEventArg);
    }
}
