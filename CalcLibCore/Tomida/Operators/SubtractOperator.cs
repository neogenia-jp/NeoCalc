using System;
using CalcLibCore.Tomida.Domain;

namespace CalcLibCore.Tomida.Operators
{
	public class SubtractOperator : ICalcOperator
	{
		public SubtractOperator()
		{
		}

        public void Calclate(CalcContextTomida ctx)
        {
            Decimal expr2 = ctx.OperandStack.Pop().ToDecimal();
            Decimal expr1 = ctx.OperandStack.Pop().ToDecimal();
            var result = Decimal.Subtract(expr1, expr2);
            ctx.OperandStack.Push(CalcNumber.Parse(result));
        }
    }
}

