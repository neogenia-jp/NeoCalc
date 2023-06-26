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
            if(ctx.GetState() == CalcConstants.State.InputLeft)
            {
                // オペランドを入れる
                ctx.OperandStack.Push(ctx.buffer);
                ctx.SubDisplayQueue.Enqueue(ctx.buffer.ToDisplayString());
                // TODO:演算子の交換の実装
                // 演算子をサブディスプレイキューとオペレーター変数に入れる
                ctx.SubDisplayQueue.Enqueue(CalcConstants.DisplayStringDic[Btn]);
                ctx.oper = Btn;
            }
            else if(ctx.GetState() == CalcConstants.State.InputRight)
            {
                ctx.OperandStack.Push(ctx.buffer);
                ctx.SubDisplayQueue.Enqueue(ctx.buffer.ToDisplayString());
                ctx.SubDisplayQueue.Enqueue(CalcConstants.DisplayStringDic[Btn]);
                var command = CalcConstants.OperatorCommandDic[ctx.oper.Value];
                command.Calclate(ctx);
                ctx.oper = Btn;
            }

            ctx.buffer = CalcNumber.Empty;

        }
    }
}

