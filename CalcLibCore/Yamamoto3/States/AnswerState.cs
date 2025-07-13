using System;

namespace CalcLib.Yamamoto3.States;

internal class AnswerState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // MainDisplayに数字を入力
        if (ctx.LeftSide.Length < 10)
        {
            ctx.LeftSide += btn.ToString().Replace("Btn", string.Empty);
        }
        ctx.State = new LeftSideState();
    }

    public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // MainDisplayの内容を左辺およびSubDisplayにセット
        ctx.LeftSide = ctx.DisplayText;
        ctx.SubDisplayText = ctx.DisplayText;
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特になし
    }
}