using System;
using System.Linq;

namespace CalcLib.Yamamoto2.Executors
{
	public class NumberButtonExecutor : ButtonExecutor
	{
		internal NumberButtonExecutor(CalcContextYamamoto2 ctx, CalcButton btn) : base(ctx, btn)
		{
        }

        public override void Execute()
        {
            if(_ctx.State == CalcContextYamamoto2.StateEnum.InputedOperator
                || _ctx.State == CalcContextYamamoto2.StateEnum.InputedEqual)
            {
                // オペレータ、= が入力された直後に数字が押された場合は、
                // 次の値の入力になるため、ディスプレイをクリアする
                _ctx.DisplayText = "";
            }

            if(_btn == CalcButton.BtnDot)
            {
                if (string.IsNullOrWhiteSpace(_ctx.DisplayText))
                {
                    // 何も入力されていない場合は0を先頭に入れておく
                    _ctx.DisplayText += "0";
                }
                else if (_ctx.DisplayText.Contains(Consts.CalcButtonText[_btn]))
                {
                    // ドットがすでに入っている場合は何もしない
                    return;
                }
            }
            _ctx.DisplayText += this.ToString();
            _ctx.State = CalcContextYamamoto2.StateEnum.InputedNumber;
        }
    }
}

