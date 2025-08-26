using CalcLib;

namespace CalcLibCore.Tomida2.Calc.Strategy
{
    /// <summary>
    /// 演算子ボタンの戦略
    /// </summary>
    internal class OperatorButtonStrategy : IButtonStrategy
    {
        private readonly char operatorChar;

        public OperatorButtonStrategy(char operatorChar)
        {
            this.operatorChar = operatorChar;
        }

        void IButtonStrategy.OnButtonClick(CalcContextTomida2 ctx, CalcButton btn)
        {
            // 操作前の状態を保存（Undo用）
            ctx.SaveState();
            
            // 結果表示後の演算子入力では、その結果から継続計算
            // （IsResultDisplayedをリセットするが、入力はクリアしない）
            if (ctx.IsDisplayingResult)
            {
                ctx.ContinueFromResult();
            }
            
            // 現在の入力を取得
            string currentInput = ctx.GetCurrentInput();
            
            // 入力が空でない場合、末尾が演算子かどうかチェック
            if (!string.IsNullOrEmpty(currentInput))
            {
                char lastChar = currentInput[currentInput.Length - 1];
                
                // 末尾が演算子の場合は置き換える
                if (IsOperator(lastChar))
                {
                    // 末尾の演算子を削除してから新しい演算子を追加
                    ctx.ClearInput();
                    ctx.AppendInput(currentInput.Substring(0, currentInput.Length - 1));
                    ctx.AppendInput(operatorChar.ToString());
                    return;
                }
            }
            
            // 通常の場合は単純に演算子を追加
            ctx.AppendInput(operatorChar.ToString());
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
