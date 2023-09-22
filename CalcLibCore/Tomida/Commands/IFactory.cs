using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
	public interface IFactory
	{
        ButtonCommandBase? Create(CalcButton btn);
    }
}

