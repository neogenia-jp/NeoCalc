using System;
namespace CalcLibCore.Tomida.Operators
{
	public class SubtractOperator : ICalcOperator
	{
		public SubtractOperator()
		{
		}

        public void Calclate(CalcContextTomida ctx)
        {
            Decimal expr2 = Decimal.Parse(ctx.OperandStack.Pop());
            Decimal expr1 = Decimal.Parse(ctx.OperandStack.Pop());
            var result = Decimal.Subtract(expr1, expr2);
            ctx.OperandStack.Push(result.ToString());
        }
    }
}

