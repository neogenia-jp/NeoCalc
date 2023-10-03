using System;
using CalcLib;
using CalcLibCore.Tomida.Commands.Switch;

namespace CalcLibCore.Tomida.Commands.Omikuji
{
	public class OmikujiCommandFactory : ButtonCommandFactoryBase
	{
        public override ButtonCommandBase? Create(CalcButton btn) => base.Create<IOmikujiCommand>(btn);
    }
}

