using System;

namespace CalcLib.Yamamoto3.States;

internal class LeftSideState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        if (ctx.LeftSide.Length < 10)
        {
            ctx.LeftSide += btn.ToString().Replace("Btn", string.Empty);
            ctx.DisplayText += btn.ToString().Replace("Btn", string.Empty);
        }
        ctx.State = new LeftSideState();
    }

    public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // subdisplayにleftを渡して、right側の入力を待つ
        ctx.Operator = btn;
        ctx.SubDisplayText = ctx.LeftSide;
        ctx.DisplayText = "";
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特に何もしない
    }
}