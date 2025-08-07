namespace CalcLib.Mori
{
    // 電卓ボタンをコマンドとして扱うinterface
    internal interface IButtonCommand
    {
        void Execute(ICalcContext ctx);
    }

    // 数値
    internal class NumberButtonCommand : IButtonCommand
    {
        readonly CalcButton _btn;
        public NumberButtonCommand(CalcButton btn)
        {
            _btn = btn;
        }

        public void Execute(ICalcContext ctx)
        {
            var context = (CalcContextExtend)ctx;
            context.Accept(_btn);
        }
    }

    // 演算子
    internal class OperatorButtonCommand : IButtonCommand
    {
        readonly CalcButton _btn;
        public OperatorButtonCommand(CalcButton btn)
        {
            _btn = btn;
        }

        public void Execute(ICalcContext ctx)
        {
            var context = (CalcContextExtend)ctx;
            context.Accept(_btn);
        }
    }

    // イコール
    internal class EqualButtonCommand : IButtonCommand
    {
        public void Execute(ICalcContext ctx)
        {
            var context = (CalcContextExtend)ctx;
            context.Accept(CalcButton.BtnEqual);
        }
    }

    // ───────── クリア / BS ─────────
    internal class ClearButtonCommand : IButtonCommand
    {
        public void Execute(ICalcContext ctx)
        {
            var context = (CalcContextExtend)ctx;
            context.Accept(CalcButton.BtnClear);
        }
    }

    internal class ClearEntryButtonCommand : IButtonCommand
    {
        public void Execute(ICalcContext ctx)
        {
            var context = (CalcContextExtend)ctx;
            context.Accept(CalcButton.BtnClearEnd);
        }
    }

    internal class BackspaceButtonCommand : IButtonCommand
    {
        public void Execute(ICalcContext ctx)
        {
            var context = (CalcContextExtend)ctx;
            context.Accept(CalcButton.BtnBS);
        }
    }

    internal class OmikujiButtonCommand : IButtonCommand
    {
        public void Execute(ICalcContext ctx)
        {
            var context = (CalcContextExtend)ctx;
            context.Accept(CalcButton.BtnExt2);
        }
    }

    // ボタンコマンドファクトリ
    // 各ボタンに対応するコマンドを生成する
    internal static class ButtonCommandFactory
    {
        public static IButtonCommand Create(CalcButton btn) => btn switch
        {
            CalcButton.Btn0 => new NumberButtonCommand(CalcButton.Btn0),
            CalcButton.Btn1 => new NumberButtonCommand(CalcButton.Btn1),
            CalcButton.Btn2 => new NumberButtonCommand(CalcButton.Btn2),
            CalcButton.Btn3 => new NumberButtonCommand(CalcButton.Btn3),
            CalcButton.Btn4 => new NumberButtonCommand(CalcButton.Btn4),
            CalcButton.Btn5 => new NumberButtonCommand(CalcButton.Btn5),
            CalcButton.Btn6 => new NumberButtonCommand(CalcButton.Btn6),
            CalcButton.Btn7 => new NumberButtonCommand(CalcButton.Btn7),
            CalcButton.Btn8 => new NumberButtonCommand(CalcButton.Btn8),
            CalcButton.Btn9 => new NumberButtonCommand(CalcButton.Btn9),
            CalcButton.BtnDot => new NumberButtonCommand(CalcButton.BtnDot),

            CalcButton.BtnPlus => new OperatorButtonCommand(CalcButton.BtnPlus),
            CalcButton.BtnMinus => new OperatorButtonCommand(CalcButton.BtnMinus),
            CalcButton.BtnMultiple => new OperatorButtonCommand(CalcButton.BtnMultiple),
            CalcButton.BtnDivide => new OperatorButtonCommand(CalcButton.BtnDivide),

            CalcButton.BtnEqual => new EqualButtonCommand(),
            CalcButton.BtnClear => new ClearButtonCommand(),
            CalcButton.BtnClearEnd => new ClearEntryButtonCommand(),
            CalcButton.BtnBS => new BackspaceButtonCommand(),
            // CalcButton.BtnExt1 => new OmikujiButtonCommand(),
            CalcButton.BtnExt2 => new OmikujiButtonCommand(),
            _ => throw new System.ArgumentOutOfRangeException(nameof(btn))
        };
    }
}       
