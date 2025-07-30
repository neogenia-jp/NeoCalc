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
            ctx.AppendInput(".");
        }
    }
}
