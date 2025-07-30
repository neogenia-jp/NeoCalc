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
            ctx.AppendInput(operatorChar.ToString());
        }
    }
}
