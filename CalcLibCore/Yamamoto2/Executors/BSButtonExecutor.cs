using System;
namespace CalcLib.Yamamoto2.Executors
{
	public class BSButtonExecutor : ButtonExecutor
	{
		internal BSButtonExecutor(CalcContextYamamoto2 ctx, CalcButton btn) : base(ctx, btn)
		{
		}

        public override void Execute()
        {
            if(_ctx.State == CalcContextYamamoto2.StateEnum.InputedNumber && _ctx.DisplayText.Length > 0)
            {
                // 末尾1文字だけ消す
                _ctx.DisplayText = _ctx.DisplayText.Substring(0, _ctx.DisplayText.Length - 1);
            }
        }
    }
}

