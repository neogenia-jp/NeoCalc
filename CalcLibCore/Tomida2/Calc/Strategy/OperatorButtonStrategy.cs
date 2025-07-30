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
            // 結果表示後の演算子入力では、その結果から継続計算
            // （IsResultDisplayedをリセットするが、入力はクリアしない）
            if (ctx.IsDisplayingResult)
            {
                ctx.ContinueFromResult();
            }
            
            ctx.AppendInput(operatorChar.ToString());
        }
    }
}
