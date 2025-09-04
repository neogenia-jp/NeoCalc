using CalcLib;
using CalcLibCore.Tomida2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalcLibCoreTest.Tomida2
{
    [TestClass]
    public class OperatorButtonStrategyTest
    {
        private CalcContextTomida2 context;

        [TestInitialize]
        public void SetUp()
        {
            context = new CalcContextTomida2();
        }

        [TestMethod]
        public void OperatorButton_SingleOperator_ShouldAddOperator()
        {
            // Arrange & Act: 1+
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);

            // Assert: 演算子が追加される
            Assert.AreEqual("1+", context.GetCurrentInput());
        }

        [TestMethod]
        public void OperatorButton_ConsecutiveOperators_ShouldReplaceLastOperator()
        {
            // Arrange: 1+2+
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.BtnPlus);

            // Act: - を追加
            context.HandleButtonClick(CalcButton.BtnMinus);

            // Assert: 最後の演算子が置き換えられる
            Assert.AreEqual("1+2-", context.GetCurrentInput());
        }

        [TestMethod]
        public void OperatorButton_MultipleConsecutiveOperators_ShouldReplaceLastOperator()
        {
            // Arrange: 1+2*
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.BtnMultiple);

            // Act: / を追加
            context.HandleButtonClick(CalcButton.BtnDivide);

            // Assert: 最後の演算子が置き換えられる
            Assert.AreEqual("1+2/", context.GetCurrentInput());
        }

        [TestMethod]
        public void OperatorButton_OperatorAfterOperand_ShouldAddOperator()
        {
            // Arrange: 123
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.Btn3);

            // Act: + を追加
            context.HandleButtonClick(CalcButton.BtnPlus);

            // Assert: 演算子が追加される
            Assert.AreEqual("123+", context.GetCurrentInput());
        }

        [TestMethod]
        public void OperatorButton_ImmediateOperatorReplacement_ShouldWork()
        {
            // Arrange: 5+
            context.HandleButtonClick(CalcButton.Btn5);
            context.HandleButtonClick(CalcButton.BtnPlus);

            // Act: * を追加（演算子の直後に別の演算子）
            context.HandleButtonClick(CalcButton.BtnMultiple);

            // Assert: 演算子が置き換えられる
            Assert.AreEqual("5*", context.GetCurrentInput());
        }

        [TestMethod]
        public void OperatorButton_ComplexExpression_ShouldReplaceOnlyLastOperator()
        {
            // Arrange: 1+2*3+
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.BtnMultiple);
            context.HandleButtonClick(CalcButton.Btn3);
            context.HandleButtonClick(CalcButton.BtnPlus);

            // Act: - を追加
            context.HandleButtonClick(CalcButton.BtnMinus);

            // Assert: 最後の演算子のみが置き換えられる
            Assert.AreEqual("1+2*3-", context.GetCurrentInput());
        }
    }
}
