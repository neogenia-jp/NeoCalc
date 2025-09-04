using System;
using CalcLib.Yamamoto3.Extensions;

namespace CalcLib.Yamamoto3.States;

internal class LeftSideState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.LeftSide += btn.ToDisplayString();
        ctx.MainDisplayManager.Concat(btn.ToDisplayString());
        ctx.State = new LeftSideState();
    }

    public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // subdisplayにleftを渡して、right側の入力を待つ
        ctx.Operator = btn;
        ctx.SubDisplayManager.Append(ctx.LeftSide);
        ctx.SubDisplayManager.Append(ctx.Operator.Value.ToDisplayString());
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特に何もしない
    }

    public void InputBs(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.LeftSide = ctx.LeftSide.DeleteLastLetter();
        ctx.MainDisplayManager.DeleteLastLetter();
    }
}