using System;
using AngleSharp.Css.Values;

namespace CalcLib.Yamamoto2.Executors
{
	public abstract class ButtonExecutor : IExecutor
	{
        internal CalcContextYamamoto2 _ctx;
        protected CalcButton _btn;

		internal ButtonExecutor(CalcContextYamamoto2 ctx, CalcButton btn)
		{
            _ctx = ctx;
            _btn = btn;
		}

        public abstract void Execute();

        public override string ToString()
        {
            return Consts.CalcButtonText[_btn];
        }
    }
}

