using System;
using CalcLibCore.Tomida.Domain;

namespace CalcLibCore.Tomida.Commands
{
	public class DivideOperator : ICalcOperator
	{
		public DivideOperator()
		{
		}

        public void Calclate(CalcContextTomida ctx)
        {
            Decimal expr2 = ctx.OperandStack.Pop().ToDecimal();
            Decimal expr1 = ctx.OperandStack.Pop().ToDecimal();
            var result = Decimal.Divide(expr1, expr2);
            ctx.OperandStack.Push(CalcNumber.Parse(result));
        }
    }
}

