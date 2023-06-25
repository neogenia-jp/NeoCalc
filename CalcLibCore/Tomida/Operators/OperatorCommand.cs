using System;
using CalcLib;
using CalcLibCore.Tomida.Domain;

namespace CalcLibCore.Tomida.Operators
{
	public class OperatorCommand : ButtonCommandBase
	{
        public OperatorCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(CalcContextTomida ctx)
        {
            ctx.OperandStack.Push(ctx.buffer);
            if (ctx.GetState() == CalcConstants.State.InputOperator)
            {
                ctx.oper = Btn;
            }
            else if (ctx.GetState() == CalcConstants.State.InputComplete)
            {
                var command = CalcConstants.OperatorCommandDic[ctx.oper.Value];
                command.Calclate(ctx);
                ctx.oper = Btn;
            }
            ctx.buffer = CalcNumber.Empty;

        }
    }
}

