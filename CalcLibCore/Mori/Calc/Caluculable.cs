namespace CalcLib.Mori
{
    // 数値や演算子を同一視して扱うcompositeパターン
    internal interface ICalculable
    {
        decimal Evaluate(); // 自分の値を計算して返す
    }

    // 数値
    internal class ValueNode : ICalculable
    {
        public decimal Value { get; }
        public ValueNode(decimal value) => Value = value;

        public decimal Evaluate() => Value; // 計算する必要がないのでそのまま返す
        public override string ToString() => Value.ToString();
    }

    // 演算子系のべースクラス
    internal abstract class OperatorNodeBase : ICalculable
    {
        protected ICalculable Left { get; } // 左辺
        protected ICalculable Right { get; } // 右辺

        protected OperatorNodeBase(ICalculable left, ICalculable right)
        {
            Left = left;
            Right = right;
        }

        public decimal Evaluate() // 左辺と右辺を計算して、その結果を計算する
        {
            var leftValue = Left.Evaluate();
            var rightValue = Right.Evaluate();
            return Calculate(leftValue, rightValue);
        }

        protected abstract decimal Calculate(decimal left, decimal right); // 具体演算
        public abstract string Symbol { get; } // 表示用
    }

    // 足し算
    internal class AdditionNode : OperatorNodeBase
    {
        public AdditionNode(ICalculable left, ICalculable right) : base(left, right) { }
        // 式プロパティ
        protected override decimal Calculate(decimal left, decimal right) => left + right;
        public override string Symbol => "+";
    }

    // 引き算
    internal class SubtractionNode : OperatorNodeBase
    {
        public SubtractionNode(ICalculable left, ICalculable right) : base(left, right) { }
        protected override decimal Calculate(decimal left, decimal right) => left - right;
        public override string Symbol => "−";
    }

    // 掛け算
    internal class MultiplicationNode : OperatorNodeBase
    {
        public MultiplicationNode(ICalculable left, ICalculable right) : base(left, right) { }
        protected override decimal Calculate(decimal left, decimal right) => left * right;
        public override string Symbol => "×";
    }

    // 割り算
    internal class DivisionNode : OperatorNodeBase
    {
        public DivisionNode(ICalculable left, ICalculable right) : base(left, right) { }
        protected override decimal Calculate(decimal left, decimal right)
        {
            if (right == 0m)
            {
                return 0m;
            }
            return left / right;
        }
        public override string Symbol => "÷";
    }
}

