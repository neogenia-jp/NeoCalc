using CalcLib;

namespace CalcLibCore.Tomida2.Calc.Strategy
{
    /// <summary>
    /// 小数点ボタンの戦略
    /// </summary>
    internal class DotButtonStrategy : IButtonStrategy
    {
        void IButtonStrategy.OnButtonClick(CalcContextTomida2 ctx, CalcButton btn)
        {
            // 操作前の状態を保存（Undo用）
            ctx.SaveState();
            
            // 結果表示後の小数点入力では新しい計算を開始
            ctx.StartNewCalculation();
            
            // 現在の入力を取得
            string currentInput = ctx.GetCurrentInput();
            
            // 最後のトークンを取得して、既に小数点が含まれているかチェック
            var lastToken = GetLastToken(currentInput);
            if (lastToken.Contains("."))
            {
                // 既に小数点が含まれている場合は無視
                return;
            }
            
            ctx.AppendInput(".");
        }

        /// <summary>
        /// 入力文字列から最後のトークンを取得します
        /// </summary>
        /// <param name="input">入力文字列</param>
        /// <returns>最後のトークン</returns>
        private string GetLastToken(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // 末尾から逆順に検索して最後のトークンを見つける
            int lastOperatorIndex = -1;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (IsOperator(input[i]))
                {
                    lastOperatorIndex = i;
                    break;
                }
            }

            if (lastOperatorIndex == -1)
            {
                // 演算子がない場合は全体が数値
                return input.Trim();
            }
            else if (lastOperatorIndex == input.Length - 1)
            {
                // 末尾が演算子
                return input[lastOperatorIndex].ToString();
            }
            else
            {
                // 最後の演算子以降が数値
                return input.Substring(lastOperatorIndex + 1).Trim();
            }
        }

        /// <summary>
        /// 指定された文字が演算子かどうかを判定します
        /// </summary>
        /// <param name="c">判定する文字</param>
        /// <returns>演算子の場合true</returns>
        private bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/';
        }
    }
}
