using CalcLib;

namespace CalcLibCore.Tomida2.Calc.Strategy
{
    /// <summary>
    /// イコールボタンの戦略
    /// </summary>
    internal class EqualButtonStrategy : IButtonStrategy
    {
        void IButtonStrategy.OnButtonClick(CalcContextTomida2 ctx, CalcButton btn)
        {
            ctx.AppendInput("=");
        }
    }
}
