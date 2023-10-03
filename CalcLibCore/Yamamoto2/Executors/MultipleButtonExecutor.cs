using System;
namespace CalcLib.Yamamoto2.Executors
{
	public class MultipleButtonExecutor : OperatorButtonExecutor
	{
		internal MultipleButtonExecutor(CalcContextYamamoto2 ctx, CalcButton btn) : base(ctx, btn)
		{
		}

        public override decimal Calculate(decimal left, decimal right)
		{
			return left * right;
		}
    }
}

