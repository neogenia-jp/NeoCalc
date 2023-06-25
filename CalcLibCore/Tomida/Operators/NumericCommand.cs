using System;
using CalcLib;

namespace CalcLibCore.Tomida.Operators
{
    public class NumericCommand : ButtonCommandBase
    {
        public NumericCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(CalcContextTomida ctx)
        {

            ctx.buffer = ctx.buffer.Append(CalcConstants.DisplayStringDic[Btn]);
        }
    }
}

