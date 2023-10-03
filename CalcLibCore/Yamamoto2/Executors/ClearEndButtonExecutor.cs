using System;
namespace CalcLib.Yamamoto2.Executors
{
	public class ClearEndButtonExecutor : ButtonExecutor
	{
		internal ClearEndButtonExecutor(CalcContextYamamoto2 ctx, CalcButton btn) : base(ctx, btn)
		{
		}

        public override void Execute()
        {
            // TODO: State管理について再考
            // 次に数字が押されたとき、DisplayTextを空っぽにしたいので、イコールと同じ扱いにしてるけど、どうなんやろ・・・。
            _ctx.SetDisplayText(0);
			_ctx.State = CalcContextYamamoto2.StateEnum.InputedEqual;
        }
    }
}

