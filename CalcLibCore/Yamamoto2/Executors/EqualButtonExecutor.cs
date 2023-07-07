using System;
namespace CalcLib.Yamamoto2.Executors
{
	public class EqualButtonExecutor : ButtonExecutor
	{
		internal EqualButtonExecutor(CalcContextYamamoto2 ctx, CalcButton btn) : base(ctx, btn)
		{
		}

        public override void Execute()
        {
            //_ctx.SubDisplayText = (_ctx.DisplayText + _text);
            var operatorExecutor = ExecutorFactory.Create(_ctx, _ctx.ope);
            operatorExecutor.Execute();
            _ctx.State = CalcContextYamamoto2.StateEnum.InputedEqual;
            _ctx.w1 = null;
        }
    }
}

