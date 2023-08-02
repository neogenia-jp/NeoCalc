using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
	public abstract class SwitchButtonCommandBase : ButtonCommandBase
	{
        protected SwitchButtonCommandBase(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(ICalcContextEx ctx) => Execute((CalcContextTomidaEx)ctx);

        public abstract void Execute(CalcContextTomidaEx ctx);

    }
}

