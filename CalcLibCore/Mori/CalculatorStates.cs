namespace CalcLib.Mori
{
    internal interface ICalcState
    {
        ICalcState AcceptInput(CalcContextExtend ctx, CalcButton btn);
    }

    // baseクラスで状態に関係なく同じ動作のボタンの動作を定義
    internal abstract class CalcStateBase : ICalcState
    {
        public virtual ICalcState AcceptInput(CalcContextExtend ctx, CalcButton btn)
        {
            if (btn.IsClear())
            {
                ctx.Reset();
                return NewNumberState.GetInstance();
            }
            else
            {
                return AcceptInputCore(ctx, btn);
            }
        }

        protected abstract ICalcState AcceptInputCore(CalcContextExtend ctx, CalcButton btn);
    }

    // 初期状態
    internal class NewNumberState : CalcStateBase
    {
        private static readonly ICalcState singleton = new NewNumberState();
        private NewNumberState() { }
        public static ICalcState GetInstance() => singleton;

        protected override ICalcState AcceptInputCore(CalcContextExtend context, CalcButton btn)
        {
            if (btn.IsNumber())
            {
                // 数値の入力し始め
                context.StartNumber(btn);
                return  NumberState.GetInstance();
            }
            else if (btn.IsOperator())
            {
                // 初期状態で演算子が押された場合、0を左辺として処理
                context.StartNumber(CalcButton.Btn0);
                context.ConfirmNumber();
                context.ProcessOperator(btn);
                return OperatorState.GetInstance();
            }
            return this;
        }
    }

    // 数値入力中
    internal class NumberState : CalcStateBase
    {
        private static readonly ICalcState singleton = new NumberState();
        private NumberState() { }
        public static ICalcState GetInstance() => singleton;

        protected override ICalcState AcceptInputCore(CalcContextExtend context, CalcButton btn)
        {
            if (btn.IsNumber())
            {
                context.AppendNumber(btn);
            }
            else if (btn.IsOperator())
            {
                context.ConfirmNumber();
                context.ProcessOperator(btn);
                return OperatorState.GetInstance();
            }
            else if (btn.IsEqual())
            {
                context.ConfirmNumber();
                context.ProcessEqual();
                return EqualState.GetInstance();
            }
            else if (btn.IsBS())
            {
                context.Backspace();
            }
            else if (btn.IsCE())
            {
                context.ClearEntry();
                return NewNumberState.GetInstance();
            }
            return this;
        }
    }

    // 直前に演算子を押した直後
    internal class OperatorState : CalcStateBase
    {
        private static readonly ICalcState singleton = new OperatorState();
        private OperatorState() { }
        public static ICalcState GetInstance() => singleton;

        protected override ICalcState AcceptInputCore(CalcContextExtend context, CalcButton btn)
        {
            if (btn.IsNumber())
            {
                context.StartNumber(btn);
                return NumberState.GetInstance();
            }
            else if (btn.IsOperator())
            {
                context.ReplaceLastOperator(btn);
            }
            else if (btn.IsEqual())
            {
                // 演算子状態で=ボタンが押された場合、左辺の値をそのまま結果として表示
                context.ProcessEqual();
                return EqualState.GetInstance();
            }
            else if (btn.IsCE())
            {
                context.ClearEntry();
                return NewNumberState.GetInstance();
            }
            return this;
        }
    }

    // 結果表示直後
    internal class EqualState : CalcStateBase
    {
        private static readonly ICalcState singleton = new EqualState();
        private EqualState() { }
        public static ICalcState GetInstance() => singleton;

        protected override ICalcState AcceptInputCore(CalcContextExtend context, CalcButton btn)
        {
            if (btn.IsNumber())
            {
                context.Reset();
                context.StartNumber(btn);
                return NumberState.GetInstance();
            }
            else if (btn.IsOperator())
            {
                // 演算子を押した場合、結果を左辺として新しい計算を開始する
                context.StartResultAsLeftOperand();
                context.ProcessOperator(btn);
                return OperatorState.GetInstance();
            }
            else if (btn.IsEqual())
            {
                // 結果表示状態で=ボタンが押された場合、前回の計算を繰り返し
                context.ProcessEqual();
            }
            return this;
        }
    }
} 