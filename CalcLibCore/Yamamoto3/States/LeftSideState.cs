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
        ctx.SubDisplayText = ctx.LeftSide;
        ctx.MainDisplayManager.Clear();
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特に何もしない
    }
}