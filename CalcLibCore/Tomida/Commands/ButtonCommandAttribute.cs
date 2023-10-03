using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ButtonCommandAttribute : Attribute
	{
		public CalcButton DependencyButton { get; }
		public ButtonCommandAttribute(CalcButton calcButton)
		{
			DependencyButton = calcButton;
		}
	}
}

