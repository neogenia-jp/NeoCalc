using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
	public interface ICalcCommand
	{
		public void Execute(ICalcContextEx ctx);
	}
}

