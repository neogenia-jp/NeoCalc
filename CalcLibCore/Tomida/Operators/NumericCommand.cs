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
            // operがEqual、かつ次の入力がオペレータではない場合、クリア処理を行う
            if (ctx.oper == CalcButton.BtnEqual)
            {
                ctx.Clear();
            }

            ctx.buffer = ctx.buffer.Append(CalcConstants.DisplayStringDic[Btn]);
            ctx.isInputed = true;
        }
    }
}

