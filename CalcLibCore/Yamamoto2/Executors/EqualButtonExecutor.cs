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
            _ctx.ope.Execute();
            _ctx.State = CalcContextYamamoto2.StateEnum.InputedEqual;
            _ctx.left = null;
            _ctx.subDisplayItems.Clear();
        }
    }
}

