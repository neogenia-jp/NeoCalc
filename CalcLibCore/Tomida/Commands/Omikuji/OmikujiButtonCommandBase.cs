using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands.Omikuji
{
	public abstract class OmikujiButtonCommandBase : ButtonCommandBase, IOmikujiCommand
    {
        protected OmikujiButtonCommandBase(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(ICalcContextEx ctx) => Execute((OmikujiContext)ctx);

        public abstract void Execute(OmikujiContext ctx);

    }
}

