using System;
using CalcLib.Yamamoto3.Extensions;

namespace CalcLib.Yamamoto3.States;

internal class OperatorState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.RightSide = btn.ToDisplayString();
        ctx.MainDisplayManager.Update(btn.ToDisplayString());
        ctx.State = new RightSideState();
    }

    public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.Operator = btn;
        ctx.SubDisplayManager.ReplaceLast(btn.ToDisplayString());
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特に何もしない
    }

    public void InputBs(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特に何もしない
    }
}
