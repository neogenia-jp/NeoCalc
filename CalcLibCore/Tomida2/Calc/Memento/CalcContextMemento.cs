namespace CalcLibCore.Tomida2.Calc.Memento
{
    /// <summary>
    /// CalcContextTomida2の状態を保存するMementoクラス
    /// </summary>
    internal class CalcContextMemento
    {
        /// <summary>
        /// 保存された入力内容
        /// </summary>
        public string RowInput { get; }

        /// <summary>
        /// 保存された結果表示状態
        /// </summary>
        public bool IsResultDisplayed { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rowInput">入力内容</param>
        /// <param name="isResultDisplayed">結果表示状態</param>
        public CalcContextMemento(string rowInput, bool isResultDisplayed)
        {
            RowInput = rowInput;
            IsResultDisplayed = isResultDisplayed;
        }
    }
}
