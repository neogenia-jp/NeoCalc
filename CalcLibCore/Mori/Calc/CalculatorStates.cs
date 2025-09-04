namespace CalcLib.Mori
{
    internal interface ICalcState
    {
        ICalcState AcceptInput(Calculator calc, CalcButton btn);
    }

    // baseクラスで状態に関係なく同じ動作のボタンの動作を定義
    internal abstract class CalcStateBase : ICalcState
    {
        public virtual ICalcState AcceptInput(Calculator calc, CalcButton btn)
        {
            if (btn.IsClear())
            {
                calc.Reset();
                return NewNumberState.GetInstance();
            }
            else
            {
                return AcceptInputCore(calc, btn);
            }
        }

        protected abstract ICalcState AcceptInputCore(Calculator calc, CalcButton btn);
    }

    // 初期状態
    internal class NewNumberState : CalcStateBase
    {
        private static readonly ICalcState singleton = new NewNumberState();
        private NewNumberState() { }
        public static ICalcState GetInstance() => singleton;

        protected override ICalcState AcceptInputCore(Calculator calc, CalcButton btn)
        {
            if (btn.IsNumber())
            {
                calc.StartNumber(btn);
                return  NumberState.GetInstance();
            }
            else if (btn.IsOperator())
            {
                // 初期状態で演算子が押された場合、0を左辺として処理
                calc.StartNumber(CalcButton.Btn0);
                calc.ConfirmNumber();
                calc.ProcessOperator(btn);
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

        protected override ICalcState AcceptInputCore(Calculator calc, CalcButton btn)
        {
            if (btn.IsNumber())
            {
                calc.AppendNumber(btn);
            }
            else if (btn.IsOperator())
            {
                calc.ConfirmNumber();
                calc.ProcessOperator(btn);
                return OperatorState.GetInstance();
            }
            else if (btn.IsBS())
            {
                calc.Backspace();
            }
            else if (btn.IsEqual())
            {
                calc.ConfirmNumber();
                calc.ProcessEqual();
                return EqualState.GetInstance();
            }
            else if (btn.IsCE())
            {
                calc.ClearEntry();
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

        protected override ICalcState AcceptInputCore(Calculator calc, CalcButton btn)
        {
            if (btn.IsNumber())
            {
                calc.StartNumber(btn);
                return NumberState.GetInstance();
            }
            else if (btn.IsOperator())
            {
                calc.ReplaceLastOperator(btn);
                return this;
            }
            else if (btn.IsBS())
            {
                // 演算子中のBSは無視する
                return this;
            }
            else if (btn.IsCE())
            {
                calc.ClearEntry();
                return NewNumberState.GetInstance();
            }
            else if (btn.IsEqual())
            {
                // 演算子状態で=ボタンが押された場合、左辺の値をそのまま結果として表示
                calc.ProcessEqual();
                return EqualState.GetInstance();
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

        protected override ICalcState AcceptInputCore(Calculator calc, CalcButton btn)
        {
            if (btn.IsNumber())
            {
                calc.Reset();
                calc.StartNumber(btn);
                return NumberState.GetInstance();
            }
            else if (btn.IsOperator())
            {
                // 演算子を押した場合、結果を左辺として新しい計算を開始する
                calc.StartResultAsLeftOperand();
                calc.ProcessOperator(btn);
                return OperatorState.GetInstance();
            }
            else if (btn.IsEqual())
            {
                // 結果表示状態で=ボタンが押された場合、前回の計算を繰り返し
                calc.ProcessEqual();
            }
            return this;
        }
    }
}

