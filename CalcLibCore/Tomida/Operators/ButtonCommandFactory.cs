using System;
using CalcLib;

namespace CalcLibCore.Tomida.Operators
{
	public class ButtonCommandFactory
	{
		public static ButtonCommandBase Create(CalcButton btn)
		{
			return new NumericCommand(btn);
		}
    }
}

