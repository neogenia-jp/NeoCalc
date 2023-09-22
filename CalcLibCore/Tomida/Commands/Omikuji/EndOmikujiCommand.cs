using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands.Omikuji
{
    [ButtonCommand(CalcButton.BtnClear)]
    [ButtonCommand(CalcButton.BtnClearEnd)]
    public class EndOmikujiCommand : OmikujiButtonCommandBase
	{
        public EndOmikujiCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(OmikujiContext ctx)
        {
            // コンテキストがおみくじならおみくじモードから抜ける
            if(ctx is OmikujiContext)
            {
                CalcContextTomidaEx ctxEx = (CalcContextTomidaEx)ctx.Parent;
                ctxEx.UnstackContext();
            }
        }
    }
}

