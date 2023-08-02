using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands.Switch
{
    public class SwitchCommandFactory : ButtonCommandFactoryBase
    {
        public override ButtonCommandBase? Create(CalcButton btn) => base.Create<ISwitchCommand>(btn);
    }
}

