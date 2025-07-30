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
            ctx.AppendInput(digit.ToString());
        }
    }
}
