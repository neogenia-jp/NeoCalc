using CalcLib;

namespace CalcLibCore.Tomida2.Calc.Strategy
{
    /// <summary>
    /// Backspaceボタン（BtnBS）の処理戦略
    /// Mementoパターンを使ったUndoを実行します
    /// </summary>
    internal class BackspaceButtonStrategy : IButtonStrategy
    {
        public void OnButtonClick(CalcContextTomida2 ctx, CalcButton btn)
        {
            // Undo機能を実行
            ctx.Undo();
        }
    }
}
