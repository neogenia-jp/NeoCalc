using System;
namespace CalcLib.Yamamoto2.Executors
{
	public abstract class OperatorButtonExecutor : ButtonExecutor
	{
		internal OperatorButtonExecutor(CalcContextYamamoto2 ctx, CalcButton btn) : base(ctx, btn)
		{
		}

        public override void Execute()
        {
            //if(_ctx.State == CalcContextYamamoto2.StateEnum.InputedOperator) {
            //    // おぺれーたが 入力 された直後 はオペレータ の切り替えになる
            //    _ctx.SubDisplayStack.pop();
            //    _ctx.SubDisplayStack.push(TextReader);
            //    return;
            //    }

            if(_ctx.w1.HasValue)
            {
                // 左辺が入っていれば、左辺とディスプレイに表示されている値をもとに計算する
                _ctx.w1 = Calculate((decimal)_ctx.w1, decimal.Parse(_ctx.DisplayText));
                _ctx.w1 = Math.Round(_ctx.w1.Value, 13);
                _ctx.DisplayText = ((decimal)_ctx.w1).ToString();
		    }
            else
            {
                // 左辺がなければ、ディスプレイに表示されている値を左辺として保持しておく
			    _ctx.w1 = decimal.Parse(_ctx.DisplayText);
                _ctx.ope = _btn;
            }
            _ctx.State = CalcContextYamamoto2.StateEnum.InputedOperator;
        }

        public abstract decimal Calculate(decimal left, decimal right);
    }
}

