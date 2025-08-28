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
        public string RawInput { get; }

        /// <summary>
        /// 保存された結果表示状態
        /// </summary>
        public bool IsResultDisplayed { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rawInput">入力内容</param>
        /// <param name="isResultDisplayed">結果表示状態</param>
        public CalcContextMemento(string rawInput, bool isResultDisplayed)
        {
            RawInput = rawInput;
            IsResultDisplayed = isResultDisplayed;
        }
    }
}
