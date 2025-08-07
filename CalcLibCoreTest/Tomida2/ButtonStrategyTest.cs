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

            Console.WriteLine("複合入力のテスト: 10+5=");
            foreach (var button in inputSequence)
            {
                ctx.HandleButtonClick(button);
                Console.WriteLine($"  {button}: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
            }
            
            // 結果の確認
            try
            {
                string displayText = ctx.DisplayText;
                Console.WriteLine($"✓ 最終結果: {displayText}");
                
                // 次の計算をテスト（結果を使った継続計算）
                Console.WriteLine("\n継続計算のテスト: +3=");
                ctx.HandleButtonClick(CalcButton.BtnPlus);
                Console.WriteLine($"  +: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
                ctx.HandleButtonClick(CalcButton.Btn3);
                Console.WriteLine($"  3: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
                ctx.HandleButtonClick(CalcButton.BtnEqual);
                Console.WriteLine($"  =: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
                
                // 新しい計算のテスト
                Console.WriteLine("\n新しい計算のテスト: 7*2=");
                ctx.HandleButtonClick(CalcButton.Btn7);
                Console.WriteLine($"  7: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
                ctx.HandleButtonClick(CalcButton.BtnMultiple);
                Console.WriteLine($"  *: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
                ctx.HandleButtonClick(CalcButton.Btn2);
                Console.WriteLine($"  2: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
                ctx.HandleButtonClick(CalcButton.BtnEqual);
                Console.WriteLine($"  =: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ エラーが発生: {ex.Message}");
            }
        }

        /// <summary>
        /// 連続演算のテスト（1+2+3=のような操作）
        /// </summary>
        public static void TestChainedOperations()
        {
            Console.WriteLine("\n=== 連続演算のテスト ===");
            var ctx = new CalcContextTomida2();
            
            // 1+2+3= のテスト
            var buttons = new[] 
            { 
                CalcButton.Btn1, CalcButton.BtnPlus, CalcButton.Btn2, 
                CalcButton.BtnPlus, CalcButton.Btn3, CalcButton.BtnEqual 
            };
            
            Console.WriteLine("連続加算のテスト: 1+2+3=");
            foreach (var button in buttons)
            {
                ctx.HandleButtonClick(button);
                Console.WriteLine($"  {button}: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
            }
            
            Console.WriteLine($"✓ 最終結果: {ctx.DisplayText}");
            
            // 2*3*4= のテスト
            ctx.ClearInput();
            var buttons2 = new[] 
            { 
                CalcButton.Btn2, CalcButton.BtnMultiple, CalcButton.Btn3, 
                CalcButton.BtnMultiple, CalcButton.Btn4, CalcButton.BtnEqual 
            };
            
            Console.WriteLine("\n連続乗算のテスト: 2*3*4=");
            foreach (var button in buttons2)
            {
                ctx.HandleButtonClick(button);
                Console.WriteLine($"  {button}: Input={ctx.GetCurrentInput()}, Display={ctx.DisplayText}");
            }
            
            Console.WriteLine($"✓ 最終結果: {ctx.DisplayText}");
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
            TestChainedOperations();
            
            Console.WriteLine("=== All Tests Completed ===");
        }
    }
}
