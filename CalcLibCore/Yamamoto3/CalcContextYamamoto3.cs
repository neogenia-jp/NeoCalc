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

		public MainDisplayManager MainDisplayManager { get; } = new MainDisplayManager();

		public SubDisplayManager SubDisplayManager { get; } = new SubDisplayManager();

        public override string DisplayText { get => MainDisplayManager.GetText(); set => throw new Exception("こいつを直接操作するな"); }

        public override string SubDisplayText { get => SubDisplayManager.GetText(); set => throw new Exception("こいつを直接操作するな"); }
    }
}

