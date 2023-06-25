using System;
using CalcLibCore.Tomida.Domain;

namespace CalcLibCore.Tomida.Operators
{
	public class MultiplyOperator : ICalcOperator
	{
		public MultiplyOperator()
		{
		}

        public void Calclate(CalcContextTomida ctx)
        {
            Decimal expr2 = ctx.OperandStack.Pop().ToDecimal();
            Decimal expr1 = ctx.OperandStack.Pop().ToDecimal();
            var result = Decimal.Multiply(expr1, expr2);
            ctx.OperandStack.Push(CalcNumber.Parse(result));
        }
    }
}

