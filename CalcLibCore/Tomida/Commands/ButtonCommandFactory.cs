using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
	public class ButtonCommandFactory
	{
		public static ButtonCommandBase Create(CalcButton btn)
		{
			if (CalcConstants.numbers.Contains(btn))
			{
				return new NumericCommand(btn);
			}
			else if (CalcConstants.operators.Contains(btn))
			{
				return new OperatorCommand(btn);
			}
			else if (CalcConstants.systemButtons.Contains(btn))
			{
				var T = _buttonFactoryDic[btn];
				var args = new Object[1] { btn };
				var instance = Activator.CreateInstance(T,args) as ButtonCommandBase;
				if(instance == null)
				{
					throw new ArgumentException("ファクトリーに登録されていないボタンです");
				}
				return instance;
			}
			else
			{
				throw new ArgumentException("ファクトリーに登録されていないボタンです");
			}
		}

		private static Dictionary<CalcButton, Type> _buttonFactoryDic = new Dictionary<CalcButton, Type>()
		{
			{ CalcButton.BtnClear, typeof(ClearCommand) },
            { CalcButton.BtnClearEnd, typeof(ClearEndCommand) },
            { CalcButton.BtnBS, typeof(BackspaceCommand) },
        };
    }
}

