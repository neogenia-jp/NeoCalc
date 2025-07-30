using CalcLib;
using System;
using System.Collections.Generic;

namespace CalcLibCore.Tomida2.Calc.Strategy
{
    /// <summary>
    /// CalcButtonに対応するIButtonStrategyを生成するファクトリ
    /// </summary>
    internal static class ButtonStrategyFactory
    {
        private static readonly Dictionary<CalcButton, IButtonStrategy> strategies = 
            new Dictionary<CalcButton, IButtonStrategy>
            {
                // 数字ボタン
                { CalcButton.Btn0, new DigitButtonStrategy('0') },
                { CalcButton.Btn1, new DigitButtonStrategy('1') },
                { CalcButton.Btn2, new DigitButtonStrategy('2') },
                { CalcButton.Btn3, new DigitButtonStrategy('3') },
                { CalcButton.Btn4, new DigitButtonStrategy('4') },
                { CalcButton.Btn5, new DigitButtonStrategy('5') },
                { CalcButton.Btn6, new DigitButtonStrategy('6') },
                { CalcButton.Btn7, new DigitButtonStrategy('7') },
                { CalcButton.Btn8, new DigitButtonStrategy('8') },
                { CalcButton.Btn9, new DigitButtonStrategy('9') },
                
                // 小数点
                { CalcButton.BtnDot, new DotButtonStrategy() },
                
                // イコール
                { CalcButton.BtnEqual, new EqualButtonStrategy() },
                
                // 演算子
                { CalcButton.BtnPlus, new OperatorButtonStrategy('+') },
                { CalcButton.BtnMinus, new OperatorButtonStrategy('-') },
                { CalcButton.BtnDivide, new OperatorButtonStrategy('/') },
                { CalcButton.BtnMultiple, new OperatorButtonStrategy('*') }
            };

        /// <summary>
        /// CalcButtonに対応するIButtonStrategyを取得
        /// </summary>
        /// <param name="button">対象のボタン</param>
        /// <returns>対応する戦略オブジェクト</returns>
        /// <exception cref="NotSupportedException">サポートされていないボタンの場合</exception>
        public static IButtonStrategy GetStrategy(CalcButton button)
        {
            if (strategies.TryGetValue(button, out var strategy))
            {
                return strategy;
            }
            
            throw new NotSupportedException($"Button {button} is not supported");
        }

        /// <summary>
        /// 指定されたボタンがサポートされているかチェック
        /// </summary>
        /// <param name="button">チェック対象のボタン</param>
        /// <returns>サポートされている場合true</returns>
        public static bool IsSupported(CalcButton button)
        {
            return strategies.ContainsKey(button);
        }
    }
}
