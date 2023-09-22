using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
    public abstract class CalcButtonCommandBase : ButtonCommandBase
    {
        protected CalcButtonCommandBase(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(ICalcContextEx ctx) => Execute((CalcContextTomida)ctx);

        public abstract void Execute(CalcContextTomida ctx);
    }
}

