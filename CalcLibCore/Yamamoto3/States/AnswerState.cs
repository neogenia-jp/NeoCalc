using System;

namespace CalcLib.Yamamoto3.States;

internal class AnswerState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 内部状態をクリアする
        ctx.LeftSide = "";
        ctx.RightSide = "";
        ctx.LeftSide = btn.ToString().Replace("Btn", string.Empty);
        ctx.DisplayText = ctx.LeftSide;

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