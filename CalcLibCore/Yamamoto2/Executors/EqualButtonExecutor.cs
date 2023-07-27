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
            if(_ctx.ope == null)
            {
                // 0.0100とか入力されていたときに0.01にしたいため、一度decimalにしてセットしなおす
                _ctx.SetDisplayText(decimal.Parse(_ctx.DisplayText));
            }
            else
            {
                _ctx.ope.Execute();
            }
            _ctx.State = CalcContextYamamoto2.StateEnum.InputedEqual;
            _ctx.left = null;
            _ctx.subDisplayItems.Clear();
        }
    }
}

