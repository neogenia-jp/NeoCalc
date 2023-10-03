using System;
using CalcLib;

namespace CalcLibCore.Tomida.Commands
{
    [ButtonCommand(CalcButton.Btn0)]
    [ButtonCommand(CalcButton.Btn1)]
    [ButtonCommand(CalcButton.Btn2)]
    [ButtonCommand(CalcButton.Btn3)]
    [ButtonCommand(CalcButton.Btn4)]
    [ButtonCommand(CalcButton.Btn5)]
    [ButtonCommand(CalcButton.Btn6)]
    [ButtonCommand(CalcButton.Btn7)]
    [ButtonCommand(CalcButton.Btn8)]
    [ButtonCommand(CalcButton.Btn9)]
    [ButtonCommand(CalcButton.BtnDot)]
    public class NumericCommand : CalcButtonCommandBase
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

