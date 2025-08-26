using CalcLib;
using CalcLibCore.Tomida2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalcLibCoreTest.Tomida2
{
    [TestClass]
    public class DisplayTextTest
    {
        private CalcContextTomida2 context;

        [TestInitialize]
        public void SetUp()
        {
            context = new CalcContextTomida2();
        }

        [TestMethod]
        public void DisplayText_SimpleOperandInput_ShouldShowOperand()
        {
            // Arrange & Act: 数字入力
            context.AppendInput("2.2");
            
            // Assert: 入力した数字が表示される
            Assert.AreEqual("2.2", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_AfterOperator_ShouldShowIntermediateResult()
        {
            // Arrange & Act: 1 + 2.2 *
            context.AppendInput("1+2.2*");
            
            // Assert: 中間結果（1+2.2=3.2）が表示される
            Assert.AreEqual("3.2", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_ComplexExpression_ShouldShowOperand()
        {
            // Arrange & Act: 1 + 2.2 * 3
            context.AppendInput("1+2.2*3");
            
            // Assert: 最後のオペランド（3）が表示される
            Assert.AreEqual("3", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_EmptyInput_ShouldShowZero()
        {
            // Arrange: 初期状態（空の入力）
            
            // Assert: "0"が表示される
            Assert.AreEqual("0", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_DebugParser_ShouldShowParserResult()
        {
            // Arrange & Act: パーサーの動作を確認
            context.AppendInput("1+2.2");
            
            // Assert: パーサーの結果を確認
            var input = context.GetCurrentInput();
            Assert.AreEqual("1+2.2", input);
            
            // DisplayTextの結果を確認
            var displayText = context.DisplayText;
            // この時点でDisplayTextがどうなっているかを確認
            System.Console.WriteLine($"Input: {input}, DisplayText: {displayText}");
            
            // パーサーエラーの詳細を確認
            try
            {
                var parser = new CalcLibCore.Tomida2.Calc.Interpreter.CalculatorParser();
                var result = parser.Parse("1+2.2*");
                System.Console.WriteLine($"Parser result for '1+2.2*': {result.Evaluate()}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Parser error for '1+2.2*': {ex.Message}");
            }
        }
    }
}
