using CalcLib;

namespace CalcLibCore.Tomida2.Calc.Strategy
{
    /// <summary>
    /// 数字ボタン（0-9）の戦略
    /// </summary>
    internal class DigitButtonStrategy : IButtonStrategy
    {
        private readonly char digit;

        public DigitButtonStrategy(char digit)
        {
            this.digit = digit;
        }

        void IButtonStrategy.OnButtonClick(CalcContextTomida2 ctx, CalcButton btn)
        {
            // 操作前の状態を保存（Undo用）
            ctx.SaveState();
            
            // 結果表示後の数字入力では新しい計算を開始
            ctx.StartNewCalculation();
            
            ctx.AppendInput(digit.ToString());
        }
    }
}
