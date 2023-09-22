using System;
namespace CalcLib.Yamamoto2.Executors
{
	public class ClearButtonExecutor : ButtonExecutor
	{
		internal ClearButtonExecutor(CalcContextYamamoto2 ctx, CalcButton btn) : base(ctx, btn)
		{
		}

        public override void Execute()
        {
            _ctx.Reset();
        }
    }
}

