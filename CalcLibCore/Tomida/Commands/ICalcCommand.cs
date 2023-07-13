using System;
namespace CalcLibCore.Tomida.Commands
{
	public interface ICalcCommand
	{
		public void Execute(CalcContextTomida ctx);
	}
}

