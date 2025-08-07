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
            
            ctx.AppendInput(".");
        }
    }
}
