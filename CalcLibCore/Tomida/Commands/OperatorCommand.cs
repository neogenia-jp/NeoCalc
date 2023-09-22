using System;
using CalcLib;
using CalcLibCore.Tomida.Domain;
using CalcLibCore.Tomida.Extensions;

namespace CalcLibCore.Tomida.Commands
{
    [ButtonCommand(CalcButton.BtnPlus)]
    [ButtonCommand(CalcButton.BtnMinus)]
    [ButtonCommand(CalcButton.BtnMultiple)]
    [ButtonCommand(CalcButton.BtnDivide)]
    [ButtonCommand(CalcButton.BtnEqual)]
    public class OperatorCommand : CalcButtonCommandBase
	{
        public OperatorCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(CalcContextTomida ctx)
        {
            if(ctx.GetState() == CalcConstants.State.InputLeft)
            {
                // オペランドを入れる
                ctx.OperandStack.Push(ctx.buffer);
                ctx.SubDisplayQueue.Add(ctx.buffer.ToDisplayString());
                // 演算子をサブディスプレイキューとオペレーター変数に入れる
                ctx.SubDisplayQueue.Add(CalcConstants.DisplayStringDic[Btn]);
                ctx.oper = Btn;
            }
            else if(ctx.GetState() == CalcConstants.State.InputRight)
            {
                ctx.OperandStack.Push(ctx.buffer);
                ctx.SubDisplayQueue.Add(ctx.buffer.ToDisplayString());
                ctx.SubDisplayQueue.Add(CalcConstants.DisplayStringDic[Btn]);
                var command = CalcConstants.OperatorCommandDic[ctx.oper.Value];
                command.Calclate(ctx);
                ctx.oper = Btn;
            }
            else if(ctx.GetState() == CalcConstants.State.InputEqual)
            {
                ctx.oper = Btn;
                ctx.SubDisplayQueue.Clear();
                ctx.SubDisplayQueue.Add(ctx.OperandStack.Peek().ToSubDisplayString());
                ctx.SubDisplayQueue.Add(Btn.ToDisplayString());
            }
            else if (ctx.GetState() == CalcConstants.State.InputOperator)
            {
                ctx.oper = Btn;
                ctx.SubDisplayQueue[ctx.SubDisplayQueue.Count - 1] = Btn.ToDisplayString();
            }

            ctx.buffer = CalcNumber.Empty;
            ctx.isInputed = false;
        }
    }
}

