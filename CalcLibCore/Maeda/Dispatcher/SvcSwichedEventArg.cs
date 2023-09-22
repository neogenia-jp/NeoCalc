namespace CalcLib.Maeda.Dispatcher
{
    /// <summary>
    /// サービス切替イベントのためのパラメータオブジェクト
    /// </summary>
    internal class SvcSwichedEventArg
    {
        /// <summary>
        /// 切り替え前のコンテキスト
        /// </summary>
        public ICalcContext PrevCtx { get; }

        public SvcSwichedEventArg(ICalcContext prevCtx)
        {
            PrevCtx = prevCtx;
        }

    }
}