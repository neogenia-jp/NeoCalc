using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcLib.Maeda.Basis
{
    /// <summary>
    /// ボタンの変換を行うことが出来るサービスアダプター
    /// </summary>
    internal class SvcBtnConvertAdapter : IBackendSvc, IEnumerable
    {
        Dictionary<CalcButton, CalcButton> Mapping = new Dictionary<CalcButton, CalcButton>();

        /// <summary>
        /// ラップしているサービス
        /// </summary>
        public IBackendSvc WrappedSvc { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="wrappedSvc"></param>
        public SvcBtnConvertAdapter(IBackendSvc wrappedSvc) { WrappedSvc = wrappedSvc; }

        /// <summary>
        /// 変換ボタンを登録する
        /// </summary>
        /// <param name="btnFrom"></param>
        /// <param name="btnTo"></param>
        public void Add(CalcButton btnFrom, CalcButton btnTo) => Mapping[btnFrom] = btnTo;

        /// <summary>
        /// 変換を行う
        /// </summary>
        /// <param name="btn">元のボタン</param>
        /// <returns>変換後のボタン</returns>
        public CalcButton Convert(CalcButton btn) => Mapping.ContainsKey(btn) ? Mapping[btn] : btn;

        public ICalcContext CreateContext() => WrappedSvc.CreateContext();

        public string GetExtButtonText(int num) => WrappedSvc.GetExtButtonText(num);

        public bool TryButtonClick(ICalcContext ctx, CalcButton btn) => WrappedSvc.TryButtonClick(ctx, Convert(btn));

        public IEnumerator GetEnumerator() => ((IEnumerable)Mapping).GetEnumerator();
    }
}
