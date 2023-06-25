using System;
using CalcLib;

namespace CalcLibCore.Tomida.Operators
{
	public class ButtonCommandFactory
	{
		public static ButtonCommandBase Create(CalcButton btn)
		{
			if (CalcConstants.numbers.Contains(btn))
			{
				return new NumericCommand(btn);
			}else if (CalcConstants.operators.Contains(btn))
			{
				return new OperatorCommand(btn);
			}
			else
			{
				throw new ArgumentException("ファクトリーに登録されていないボタンです");
			}
		}
    }
}

