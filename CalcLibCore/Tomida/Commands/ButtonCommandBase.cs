using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
	public abstract class ButtonCommandBase : ICalcCommand
	{
		public CalcButton Btn { get; }

		public string BtnText => CalcConstants.DisplayStringDic[Btn];

		public ButtonCommandBase(CalcButton btn)
		{
			Btn = btn;
		}

        public abstract void Execute(ICalcContextEx ctx);

    }
}

