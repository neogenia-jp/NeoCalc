namespace CalcLib.Takao
{
    internal class CalcContext : ICalcContext
    {
        public CalcContext()
        {
            digits.Push("0");
        }

        // 入力数字を管理
        public Stack<string> digits = new Stack<string>();

        // 計算するクラスを保持
        // operatorはStrategyパターン
        public ICalcStrategy? operatorMode;

        // 入力状態の一覧
        public enum State
        {
            First,
            Second,
            Equal,
        }

        // 入力状態の管理
        public State state { get; set; } = State.First;

        // Displayに表示する文字
        public string DisplayText
        {
            get
            {
                Console.WriteLine(digits.Count());
                Console.WriteLine(digits.Peek());
                return FormatDisplayText(digits.Peek());
            }
        }

        public string FormatDisplayText(string text)
        {
            return Decimal.Parse(text).ToString("0.#############");
        }

        // subDisplayに表示する文字
        public string SubDisplayText
        {
            get
            {
                return state.ToString();
            }
        }
    }
}
