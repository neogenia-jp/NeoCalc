using System;
using CalcLibCore.Tomida.Domain;
namespace CalcLibCore.Tomida.Commands
{
	public class AddOperator : ICalcOperator
	{
		public AddOperator()
		{
		}

        public void Calclate(CalcContextTomida ctx)
        {
            Decimal expr2 = ctx.OperandStack.Pop().ToDecimal();
            Decimal expr1 = ctx.OperandStack.Pop().ToDecimal();
            var result = Decimal.Add(expr1, expr2);
            ctx.OperandStack.Push(CalcNumber.Parse(result));
        }
    }
}

