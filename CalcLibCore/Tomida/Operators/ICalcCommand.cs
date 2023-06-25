using System;
namespace CalcLibCore.Tomida.Operators
{
	public interface ICalcCommand
	{
		public void Execute(CalcContextTomida ctx);
	}
}

