using System;
using CalcLib.Yamamoto3.Extensions;

namespace CalcLib.Yamamoto3.States;

internal class InitState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.LeftSide += btn.ToDisplayString();
        ctx.MainDisplayManager.Update(btn.ToDisplayString());
        ctx.State = new LeftSideState();
    }

    public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特に何もしない
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特に何もしない
    }
}