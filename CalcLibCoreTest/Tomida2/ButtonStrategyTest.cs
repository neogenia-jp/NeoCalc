using CalcLib;
using CalcLibCore.Tomida2;
using CalcLibCore.Tomida2.Calc.Strategy;
using System;
using System.Diagnostics;

namespace CalcLibCoreTest.Tomida2
{
    /// <summary>
    /// ボタンストラテジのテストクラス
    /// </summary>
    public class ButtonStrategyTest
    {
        /// <summary>
        /// 数字ボタンのテスト
        /// </summary>
        public static void TestDigitButtons()
        {
            var ctx = new CalcContextTomida2();
            
            // 数字ボタンをテスト
            var buttons = new[] 
            { 
                CalcButton.Btn0, CalcButton.Btn1, CalcButton.Btn2, CalcButton.Btn3, CalcButton.Btn4,
                CalcButton.Btn5, CalcButton.Btn6, CalcButton.Btn7, CalcButton.Btn8, CalcButton.Btn9
            };

            foreach (var button in buttons)
            {
                ctx.ClearInput();
                var strategy = ButtonStrategyFactory.GetStrategy(button);
                strategy.OnButtonClick(ctx, button);
                
                string expected = button.ToString().Replace("Btn", "");
                string actual = ctx.GetCurrentInput();
                
                Debug.Assert(actual == expected, $"Expected {expected}, but got {actual} for {button}");
                Console.WriteLine($"✓ {button}: {actual}");
            }
        }

        /// <summary>
        /// 小数点ボタンのテスト
        /// </summary>
        public static void TestDotButton()
        {
            var ctx = new CalcContextTomida2();
            var strategy = ButtonStrategyFactory.GetStrategy(CalcButton.BtnDot);
            
            strategy.OnButtonClick(ctx, CalcButton.BtnDot);
            string actual = ctx.GetCurrentInput();
            
            Debug.Assert(actual == ".", $"Expected '.', but got '{actual}'");
            Console.WriteLine($"✓ BtnDot: {actual}");
        }

        /// <summary>
        /// 演算子ボタンのテスト
        /// </summary>
        public static void TestOperatorButtons()
        {
            var ctx = new CalcContextTomida2();
            var operatorTests = new[]
            {
                (CalcButton.BtnPlus, "+"),
                (CalcButton.BtnMinus, "-"),
                (CalcButton.BtnDivide, "/"),
                (CalcButton.BtnMultiple, "*")
            };

            foreach (var (button, expected) in operatorTests)
            {
                ctx.ClearInput();
                var strategy = ButtonStrategyFactory.GetStrategy(button);
                strategy.OnButtonClick(ctx, button);
                
                string actual = ctx.GetCurrentInput();
                Debug.Assert(actual == expected, $"Expected {expected}, but got {actual} for {button}");
                Console.WriteLine($"✓ {button}: {actual}");
            }
        }

        /// <summary>
        /// イコールボタンのテスト
        /// </summary>
        public static void TestEqualButton()
        {
            var ctx = new CalcContextTomida2();
            var strategy = ButtonStrategyFactory.GetStrategy(CalcButton.BtnEqual);
            
            strategy.OnButtonClick(ctx, CalcButton.BtnEqual);
            string actual = ctx.GetCurrentInput();
            
            Debug.Assert(actual == "=", $"Expected '=', but got '{actual}'");
            Console.WriteLine($"✓ BtnEqual: {actual}");
        }

        /// <summary>
        /// 複合入力のテスト（実際の計算機操作のシミュレーション）
        /// </summary>
        public static void TestComplexInput()
        {
            var ctx = new CalcContextTomida2();
            
            // "10+5=" の入力をシミュレート
            var inputSequence = new[]
            {
                CalcButton.Btn1,
                CalcButton.Btn0,
                CalcButton.BtnPlus,
                CalcButton.Btn5,
                CalcButton.BtnEqual
            };

            foreach (var button in inputSequence)
            {
                var strategy = ButtonStrategyFactory.GetStrategy(button);
                strategy.OnButtonClick(ctx, button);
            }

            string actual = ctx.GetCurrentInput();
            string expected = "10+5=";
            
            Debug.Assert(actual == expected, $"Expected {expected}, but got {actual}");
            Console.WriteLine($"✓ Complex input: {actual}");
            
            // DisplayTextも確認（パーサーによる評価結果）
            try
            {
                string displayText = ctx.DisplayText;
                Console.WriteLine($"✓ Display result: {displayText}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Display evaluation failed: {ex.Message}");
            }
        }

        /// <summary>
        /// すべてのテストを実行
        /// </summary>
        public static void RunAllTests()
        {
            Console.WriteLine("=== Button Strategy Tests ===");
            
            TestDigitButtons();
            TestDotButton();
            TestOperatorButtons();
            TestEqualButton();
            TestComplexInput();
            
            Console.WriteLine("=== All Tests Completed ===");
        }
    }
}
