using CalcLib;
using CalcLibCore.Tomida2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalcLibCoreTest.Tomida2
{
    [TestClass]
    public class DisplayTextFormatTest
    {
        private CalcContextTomida2 context;

        [TestInitialize]
        public void SetUp()
        {
            context = new CalcContextTomida2();
        }

        [TestMethod]
        public void DisplayText_IntegerWithThousands_ShouldShowCommasSeparation()
        {
            // Arrange & Act: 1234を入力
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.Btn3);
            context.HandleButtonClick(CalcButton.Btn4);

            // Assert: 1,234と表示される
            Assert.AreEqual("1,234", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_LargeInteger_ShouldShowCommasSeparation()
        {
            // Arrange & Act: 1234567を入力
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.Btn3);
            context.HandleButtonClick(CalcButton.Btn4);
            context.HandleButtonClick(CalcButton.Btn5);
            context.HandleButtonClick(CalcButton.Btn6);
            context.HandleButtonClick(CalcButton.Btn7);

            // Assert: 1,234,567と表示される
            Assert.AreEqual("1,234,567", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_DecimalWithThousands_ShouldShowCommasInIntegerPart()
        {
            // Arrange & Act: 1234.56を入力
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.Btn3);
            context.HandleButtonClick(CalcButton.Btn4);
            context.HandleButtonClick(CalcButton.BtnDot);
            context.HandleButtonClick(CalcButton.Btn5);
            context.HandleButtonClick(CalcButton.Btn6);

            // Assert: 整数部分にカンマ、小数部分はそのまま
            Assert.AreEqual("1,234.56", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_NegativeIntegerWithThousands_ShouldShowCommasWithMinus()
        {
            // Arrange & Act: 5000 - 6234 = (-1234)の結果を表示
            context.HandleButtonClick(CalcButton.Btn5);
            context.HandleButtonClick(CalcButton.Btn0);
            context.HandleButtonClick(CalcButton.Btn0);
            context.HandleButtonClick(CalcButton.Btn0);
            context.HandleButtonClick(CalcButton.BtnMinus);
            context.HandleButtonClick(CalcButton.Btn6);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.Btn3);
            context.HandleButtonClick(CalcButton.Btn4);
            context.HandleButtonClick(CalcButton.BtnEqual);

            // Assert: 負数もカンマ区切りで表示
            Assert.AreEqual("-1,234", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_SmallInteger_ShouldNotShowCommas()
        {
            // Arrange & Act: 123を入力
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.Btn3);

            // Assert: 千未満はカンマなし
            Assert.AreEqual("123", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_CalculationResult_ShouldShowCommasInResult()
        {
            // Arrange & Act: 123 + 1000 = 1123
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.Btn2);
            context.HandleButtonClick(CalcButton.Btn3);
            context.HandleButtonClick(CalcButton.BtnPlus);
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.Btn0);
            context.HandleButtonClick(CalcButton.Btn0);
            context.HandleButtonClick(CalcButton.Btn0);
            context.HandleButtonClick(CalcButton.BtnEqual);

            // Assert: 計算結果もカンマ区切り
            Assert.AreEqual("1,123", context.DisplayText);
        }

        [TestMethod]
        public void DisplayText_IntermediateResult_ShouldShowCommasInIntermediate()
        {
            // Arrange & Act: 1500 + (演算子の後の中間結果表示)
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.Btn5);
            context.HandleButtonClick(CalcButton.Btn0);
            context.HandleButtonClick(CalcButton.Btn0);
            context.HandleButtonClick(CalcButton.BtnPlus);

            // Assert: 中間結果もカンマ区切り
            Assert.AreEqual("1,500", context.DisplayText);
        }
    }
}
