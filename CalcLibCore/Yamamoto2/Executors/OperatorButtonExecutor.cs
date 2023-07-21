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

            if (_ctx.left.HasValue)
            {
                // ディスプレイに表示されている値をサブディスプレイに表示する
                _ctx.subDisplayItems.Add(decimal.Parse(_ctx.DisplayText).ToString());

                // 左辺が入っていれば、左辺とディスプレイに表示されている値をもとに計算する
                _ctx.left = _ctx.ope.Calculate((decimal)_ctx.left, decimal.Parse(_ctx.DisplayText));
                _ctx.left = Math.Round(_ctx.left.Value, 13);
                _ctx.SetDisplayText((decimal)_ctx.left);
		    }
            else
            {
                // 左辺がなければ、ディスプレイに表示されている値を左辺として保持しておく
			    _ctx.left = decimal.Parse(_ctx.DisplayText);
                _ctx.subDisplayItems.Add(((decimal)_ctx.left).ToString());
            }
            _ctx.ope = this;
            _ctx.subDisplayItems.Add(this.ToString());
            _ctx.State = CalcContextYamamoto2.StateEnum.InputedOperator;
        }

        public abstract decimal Calculate(decimal left, decimal right);
    }
}

