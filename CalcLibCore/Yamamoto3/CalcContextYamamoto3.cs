using System;
using System.Runtime.CompilerServices;
using CalcLib.Yamamoto3.States;

namespace CalcLib.Yamamoto3
{
	internal class CalcContextYamamoto3 : CalcContext
	{
		public IState State { get; set; } = new InitState();
		public string LeftSide { get; set; } = string.Empty;
		public string RightSide { get; set; } = string.Empty;
		public CalcButton? Operator { get; set; } = null;
    }
}

