using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
    [ButtonCommand(CalcButton.BtnBS)]
    public class BackspaceCommand : ButtonCommandBase
	{
        public BackspaceCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(CalcContextTomida ctx)
        {
            ctx.buffer = ctx.buffer.Backward();
        }

        public static new IEnumerable<CalcButton>? DependencyButtons
        {
            get
            {
                return new CalcButton[]
                {
                    CalcButton.BtnBS
                };
            }
        }

    }
}

