using System;
namespace CalcLib.Yamamoto2.Executors
{
	public class DivideButtonExecutor : OperatorButtonExecutor
	{
		internal DivideButtonExecutor(CalcContextYamamoto2 ctx, CalcButton btn) : base(ctx, btn)
		{
		}

        public override decimal Calculate(decimal left, decimal right)
		{
			return Math.Round(left / right, 13);
		}
    }
}

