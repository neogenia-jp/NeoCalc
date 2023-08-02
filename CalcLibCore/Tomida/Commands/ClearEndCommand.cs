using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
    [ButtonCommand(CalcButton.BtnClearEnd)]
	public class ClearEndCommand : CalcButtonCommandBase
    {
        public ClearEndCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(CalcContextTomida ctx)
        {
            ctx.isInputed = true;
            ctx.buffer = Domain.CalcNumber.Empty;
        }
    }
}

