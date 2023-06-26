using System;
using CalcLib;

namespace CalcLibCore.Tomida.Operators
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

