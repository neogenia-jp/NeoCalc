
using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
    [ButtonCommand(CalcButton.BtnClear)]
    public class ClearCommand : CalcButtonCommandBase
    {
        public ClearCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(CalcContextTomida ctx)
        {
            ctx.Clear();
        }
    }
}

