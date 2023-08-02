using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands.Switch
{
    [ButtonCommand(CalcButton.BtnExt2)]
	public class OmikujiSwitchCommand: SwitchButtonCommandBase, ISwitchCommand
	{
		public OmikujiSwitchCommand(CalcButton btn) : base(btn)
        {
		}

        public override void Execute(CalcContextTomidaEx ctx)
        {
            // 現在のコンテキストがおみくじだったらおみくじから抜け、そうでないならおみくじコンテキストをスタックする
            if(ctx.Current is OmikujiContext)
            {
                ctx.UnstackContext();
            }
            else
            {
                ctx.StackContext(new OmikujiContext());
            }
        }
    }
}

