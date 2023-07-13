
using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
    public class ClearCommand : ButtonCommandBase
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

