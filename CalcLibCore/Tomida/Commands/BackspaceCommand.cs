using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
    public class BackspaceCommand : ButtonCommandBase
	{
        public BackspaceCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(CalcContextTomida ctx)
        {
            ctx.buffer = ctx.buffer.Backward();
        }
    }
}

