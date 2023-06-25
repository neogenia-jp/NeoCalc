using System;
using CalcLib;

namespace CalcLibCore.Tomida.Operators
{
	public abstract class ButtonCommandBase : ICalcCommand
	{
		public CalcButton Btn { get; }

		public string BtnText => CalcConstants.DisplayStringDic[Btn];

		public ButtonCommandBase(CalcButton btn)
		{
			Btn = btn;
		}

        public abstract void Execute(CalcContextTomida ctx);
    }
}

