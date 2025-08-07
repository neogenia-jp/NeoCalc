using CalcLib;
using CalcLibCore.Tomida2.Calc.Interpreter;

namespace CalcLibCore.Tomida2.Calc.Strategy
{
    /// <summary>
    /// イコールボタンの戦略
    /// </summary>
    internal class EqualButtonStrategy : IButtonStrategy
    {
        void IButtonStrategy.OnButtonClick(CalcContextTomida2 ctx, CalcButton btn)
        {
            // 操作前の状態を保存（Undo用）
            ctx.SaveState();
            
            // 現在の入力があるかチェック
            var currentInput = ctx.GetCurrentInput();
            if (string.IsNullOrEmpty(currentInput))
                return;

            try
            {
                // パーサーで解析して計算実行
                var parser = new CalculatorParser();
                var result = parser.Parse(currentInput);
                var value = result.Evaluate();
                
                // 結果を適切な精度でフォーマット
                string formattedValue;
                if (value != System.Math.Floor(value))
                {
                    formattedValue = System.Math.Round(value, 13).ToString();
                }
                else
                {
                    formattedValue = value.ToString();
                }
                
                // 結果を表示状態に設定
                ctx.SetResult(formattedValue);
            }
            catch
            {
                // パースエラーの場合は何もしない
            }
        }
    }
}
