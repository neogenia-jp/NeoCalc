using System;
using CalcLib;
using CalcLibCore.Tomida.Commands.Switch;

namespace CalcLibCore.Tomida.Commands
{
	public abstract class SwitchButtonCommandBase : ButtonCommandBase, ISwitchCommand
    {
        protected SwitchButtonCommandBase(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(ICalcContextEx ctx) => Execute((CalcContextTomidaEx)ctx);

        public abstract void Execute(CalcContextTomidaEx ctx);

    }
}

