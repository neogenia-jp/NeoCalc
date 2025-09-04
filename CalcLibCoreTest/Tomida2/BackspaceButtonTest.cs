using CalcLib;
using CalcLibCore.Tomida2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalcLibCoreTest.Tomida2
{
    [TestClass]
    public class BackspaceButtonTest
    {
        private CalcContextTomida2 context;

        [TestInitialize]
        public void SetUp()
        {
            context = new CalcContextTomida2();
        }

        [TestMethod]
        public void BtnBS_WithPreviousState_ShouldUndoLastOperation()
        {
            // Arrange: 数字を入力
            context.HandleButtonClick(CalcButton.Btn1);
            var stateAfterFirst = context.DisplayText;
            
            context.HandleButtonClick(CalcButton.Btn2);
            var stateAfterSecond = context.DisplayText;

            // Act: Backspace（Undo）を実行
            context.HandleButtonClick(CalcButton.BtnBS);

            // Assert: 一つ前の状態に戻る
            Assert.AreEqual(stateAfterFirst, context.DisplayText);
        }

        [TestMethod]
        public void BtnBS_MultipleOperations_ShouldUndoInReverseOrder()
        {
            // Arrange: 複数の操作を実行
            context.HandleButtonClick(CalcButton.Btn1);
            var state1 = context.DisplayText;
            
            context.HandleButtonClick(CalcButton.BtnPlus);
            var state2 = context.DisplayText;
            
            context.HandleButtonClick(CalcButton.Btn2);
            var state3 = context.DisplayText;

            // Act & Assert: 逆順でUndo
            context.HandleButtonClick(CalcButton.BtnBS);
            Assert.AreEqual(state2, context.DisplayText);
            
            context.HandleButtonClick(CalcButton.BtnBS);
            Assert.AreEqual(state1, context.DisplayText);
            
            context.HandleButtonClick(CalcButton.BtnBS);
            Assert.AreEqual("0", context.DisplayText); // 初期状態
        }

        [TestMethod]
        public void BtnBS_WithCalculation_ShouldUndoToBeforeEqual()
        {
            // Arrange: 計算を実行
            context.HandleButtonClick(CalcButton.Btn1);
            context.HandleButtonClick(CalcButton.BtnPlus);
            context.HandleButtonClick(CalcButton.Btn2);
            var stateBeforeEqual = context.DisplayText;
            
            context.HandleButtonClick(CalcButton.BtnEqual);
            Assert.AreEqual("3", context.DisplayText); // 計算結果確認

            // Act: Backspace（Undo）を実行
            context.HandleButtonClick(CalcButton.BtnBS);

            // Assert: イコールボタン押下前の状態に戻る
            Assert.AreEqual(stateBeforeEqual, context.DisplayText);
        }

        [TestMethod]
        public void BtnBS_WithNoHistory_ShouldNotCrash()
        {
            // Arrange: 初期状態（履歴なし）
            var initialState = context.DisplayText;

            // Act: Backspace（Undo）を実行
            context.HandleButtonClick(CalcButton.BtnBS);

            // Assert: クラッシュせず、状態が変わらない
            Assert.AreEqual(initialState, context.DisplayText);
        }

        [TestMethod]
        public void BtnBS_CanUndoProperty_ShouldReflectHistoryState()
        {
            // Arrange: 初期状態では履歴なし
            Assert.IsFalse(context.CanUndo);

            // Act: 操作を実行
            context.HandleButtonClick(CalcButton.Btn1);
            
            // Assert: 履歴があるためUndoが可能
            Assert.IsTrue(context.CanUndo);
            
            // Act: Undoを実行
            context.HandleButtonClick(CalcButton.BtnBS);
            
            // Assert: 履歴がなくなったためUndoが不可能
            Assert.IsFalse(context.CanUndo);
        }
    }
}
