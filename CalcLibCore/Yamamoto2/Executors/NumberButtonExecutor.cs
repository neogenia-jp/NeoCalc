using System;
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

            if(_btn == CalcButton.BtnDot && _ctx.DisplayText.Last().ToString() == Consts.CalcButtonText[_btn])
            {
                // ドットが押された場合、ドットを2個続けないように処理を終える
                return;
            }
            _ctx.DisplayText += this.ToString();
            _ctx.State = CalcContextYamamoto2.StateEnum.InputedNumber;
        }
    }
}

