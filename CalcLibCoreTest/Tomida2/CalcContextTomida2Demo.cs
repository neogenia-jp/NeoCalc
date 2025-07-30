using CalcLib;
using CalcLibCore.Tomida2;
using System;

namespace CalcLibCoreTest.Tomida2
{
    /// <summary>
    /// CalcContextTomida2とボタンストラテジの使用例デモ
    /// </summary>
    public class CalcContextTomida2Demo
    {
        public static void Main()
        {
            Console.WriteLine("=== CalcContextTomida2 Demo ===");

            var calc = new CalcContextTomida2();

            Console.WriteLine("\n--- 基本的な計算: 12+34= ---");
            DemoCalculation(calc, new[] 
            { 
                CalcButton.Btn1, CalcButton.Btn2, CalcButton.BtnPlus,
                CalcButton.Btn3, CalcButton.Btn4, CalcButton.BtnEqual 
            });

            Console.WriteLine("\n--- 小数点を含む計算: 3.14*2= ---");
            calc.ClearInput();
            DemoCalculation(calc, new[] 
            { 
                CalcButton.Btn3, CalcButton.BtnDot, CalcButton.Btn1, CalcButton.Btn4,
                CalcButton.BtnMultiple, CalcButton.Btn2, CalcButton.BtnEqual 
            });

            Console.WriteLine("\n--- 除算: 100/4= ---");
            calc.ClearInput();
            DemoCalculation(calc, new[] 
            { 
                CalcButton.Btn1, CalcButton.Btn0, CalcButton.Btn0,
                CalcButton.BtnDivide, CalcButton.Btn4, CalcButton.BtnEqual 
            });

            Console.WriteLine("\n--- 減算: 50-25= ---");
            calc.ClearInput();
            DemoCalculation(calc, new[] 
            { 
                CalcButton.Btn5, CalcButton.Btn0, CalcButton.BtnMinus,
                CalcButton.Btn2, CalcButton.Btn5, CalcButton.BtnEqual 
            });
        }

        private static void DemoCalculation(CalcContextTomida2 calc, CalcButton[] buttons)
        {
            string inputSequence = "";
            
            foreach (var button in buttons)
            {
                calc.HandleButtonClick(button);
                inputSequence += ButtonToString(button);
                
                Console.WriteLine($"  Button: {button,12} | Input: {calc.GetCurrentInput(),-10} | Display: {GetDisplaySafe(calc)}");
            }
            
            Console.WriteLine($"  Final Input: {calc.GetCurrentInput()}");
            Console.WriteLine($"  Final Result: {GetDisplaySafe(calc)}");
        }

        private static string ButtonToString(CalcButton button)
        {
            return button switch
            {
                CalcButton.Btn0 => "0",
                CalcButton.Btn1 => "1",
                CalcButton.Btn2 => "2",
                CalcButton.Btn3 => "3",
                CalcButton.Btn4 => "4",
                CalcButton.Btn5 => "5",
                CalcButton.Btn6 => "6",
                CalcButton.Btn7 => "7",
                CalcButton.Btn8 => "8",
                CalcButton.Btn9 => "9",
                CalcButton.BtnDot => ".",
                CalcButton.BtnPlus => "+",
                CalcButton.BtnMinus => "-",
                CalcButton.BtnMultiple => "*",
                CalcButton.BtnDivide => "/",
                CalcButton.BtnEqual => "=",
                _ => button.ToString()
            };
        }

        private static string GetDisplaySafe(CalcContextTomida2 calc)
        {
            try
            {
                return calc.DisplayText;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
