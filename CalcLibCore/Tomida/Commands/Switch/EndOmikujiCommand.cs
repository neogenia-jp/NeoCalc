using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands.Omikuji
{
    [ButtonCommand(CalcButton.BtnClear)]
    [ButtonCommand(CalcButton.BtnClearEnd)]
    public class EndOmikujiCommand : SwitchButtonCommandBase
	{
        public EndOmikujiCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(CalcContextTomidaEx ctx)
        {
            // コンテキストがおみくじならおみくじモードから抜ける
            if(ctx.Current is OmikujiContext)
            {
                ctx.UnstackContext();
            }
        }
    }
}

