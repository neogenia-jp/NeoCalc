using CalcLib;
using CalcLibCore.Tomida2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalcLibCoreTest.Tomida2
{
    [TestClass]
    public class SubDisplayTextTest
    {
        private CalcContextTomida2 context;

        [TestInitialize]
        public void SetUp()
        {
            context = new CalcContextTomida2();
        }

        [TestMethod]
        public void SubDisplayText_EmptyInput_ShouldReturnEmpty()
        {
            // Arrange: 初期状態（空の入力）
            
            // Act & Assert
            Assert.AreEqual(string.Empty, context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_SimpleExpression_ShouldFormatWithSpacesButNotShowTrailingOperand()
        {
            // Arrange: 簡単な式を入力 "1+2"
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);
            context.HandleButtonClick(CalcButton.Btn2);

            // Act & Assert: 末尾のオペランド"2"は表示せず、"1 +"のみ表示
            Assert.AreEqual("1 +", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_ComplexExpression_ShouldFormatAllOperatorsButNotShowTrailingOperand()
        {
            // Arrange: 複雑な式を手動で構築 "1+23*45"
            context.AppendInput("1+23*45");

            // Act & Assert: 末尾のオペランド"45"は表示せず、"1 + 23 ×"のみ表示
            Assert.AreEqual("1 + 23 ×", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_MultipleOperators_ShouldFormatCorrectlyWithoutTrailingOperand()
        {
            // Arrange: 複数の演算子を含む式 "10-5/2+3"
            context.AppendInput("10-5/2+3");

            // Act & Assert: 末尾のオペランド"3"は表示せず、"10 - 5 ÷ 2 +"のみ表示
            Assert.AreEqual("10 - 5 ÷ 2 +", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_WithDecimal_ShouldFormatCorrectlyWithoutTrailingOperand()
        {
            // Arrange: 小数点を含む式 "3.14*2.5"
            context.AppendInput("3.14*2.5");

            // Act & Assert: 末尾のオペランド"2.5"は表示せず、"3.14 ×"のみ表示
            Assert.AreEqual("3.14 ×", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_SingleOperand_ShouldReturnEmpty()
        {
            // Arrange: 単一のオペランド（"1"）
            context.HandleButtonClick(CalcButton.Btn1);

            // Act & Assert: 末尾がオペランドの場合は空文字を返す
            Assert.AreEqual(string.Empty, context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_OperandAfterOperator_ShouldNotShowTrailingOperand()
        {
            // Arrange: "1+"
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);

            // Act & Assert: 演算子で終わる場合は表示
            Assert.AreEqual("1 +", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_OperandAfterOperatorThenOperand_ShouldNotShowTrailingOperand()
        {
            // Arrange: "1+2"
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);
            context.HandleButtonClick(CalcButton.Btn2);

            // Act & Assert: 末尾のオペランドは表示しない（"1+"のみ表示）
            Assert.AreEqual("1 +", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_ComplexExpressionWithTrailingOperand_ShouldNotShowTrailingOperand()
        {
            // Arrange: "1+2*3"のような複雑な式
            context.AppendInput("1+2*3");

            // Act & Assert: 末尾のオペランド"3"は表示せず、"1 + 2 ×"のみ表示
            Assert.AreEqual("1 + 2 ×", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_JustOperator_ShouldShowOperatorWithSpaces()
        {
            // Arrange: "1+"（演算子で終わる）
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);

            // Act & Assert: 演算子で終わる場合は表示
            Assert.AreEqual("1 +", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_MultipleOperatorsInSequence_ShouldShowLastOperator()
        {
            // Arrange: "1+*"（複数の演算子）
            context.AppendInput("1+*");

            // Act & Assert: 最後の演算子まで表示
            Assert.AreEqual("1 + ×", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_WithEqualSign_ShouldReturnEmpty()
        {
            // Arrange: "1+2="
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.BtnEqual);

            // Act & Assert: 末尾が=の場合は空文字を返す
            Assert.AreEqual(string.Empty, context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_NewSpecificationExamples_ShouldMatchExpectedBehavior()
        {
            // Test case: "1" => ""
            context.AppendInput("1");
            Assert.AreEqual("", context.SubDisplayText);

            // Reset and test: "1+" => "1 +"
            context = new CalcContextTomida2();
            context.AppendInput("1+");
            Assert.AreEqual("1 +", context.SubDisplayText);

            // Reset and test: "1+2" => "1 +"
            context = new CalcContextTomida2();
            context.AppendInput("1+2");
            Assert.AreEqual("1 +", context.SubDisplayText);

            // Reset and test: "1+2=" => ""
            context = new CalcContextTomida2();
            context.AppendInput("1+2=");
            Assert.AreEqual("", context.SubDisplayText);
        }

        [TestMethod]
        public void SubDisplayText_CalcSvc2Scenario_ShouldMatchExpectedBehavior()
        {
            // Simulate CalcSvcTest2 scenario step by step
            
            // 1
            context.AppendInput("1");
            Assert.AreEqual("", context.SubDisplayText, "Step 1: '1' should show ''");

            // 1+
            context = new CalcContextTomida2();
            context.AppendInput("1+");
            Assert.AreEqual("1 +", context.SubDisplayText, "Step 2: '1+' should show '1 +'");

            // 1+2.2
            context = new CalcContextTomida2();
            context.AppendInput("1+2.2");
            Assert.AreEqual("1 +", context.SubDisplayText, "Step 3: '1+2.2' should show '1 +'");

            // 1+2.2*
            context = new CalcContextTomida2();
            context.AppendInput("1+2.2*");
            Assert.AreEqual("1 + 2.2 ×", context.SubDisplayText, "Step 4: '1+2.2*' should show '1 + 2.2 ×'");

            // 1+2.2*3-
            context = new CalcContextTomida2();
            context.AppendInput("1+2.2*3-");
            Assert.AreEqual("1 + 2.2 × 3 -", context.SubDisplayText, "Step 5: '1+2.2*3-' should show '1 + 2.2 × 3 -'");
        }

        [TestMethod]
        public void SubDisplayText_ResultDisplayed_ShouldReturnEmpty()
        {
            // Arrange: 計算結果を表示状態にする
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.BtnEqual);

            // Act & Assert: 結果表示中は空文字を返す
            Assert.AreEqual(string.Empty, context.SubDisplayText);
        }
    }
}
