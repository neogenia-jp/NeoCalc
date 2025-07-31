using System;
using CalcLib.Yamamoto3.Extensions;

namespace CalcLib.Yamamoto3.States;

internal class RightSideState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.RightSide += btn.ToDisplayString();
        ctx.MainDisplayManager.Concat(btn.ToDisplayString());
        ctx.State = new RightSideState();
    }

    public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // まだ右側の入力がない場合はOperatorが変更されたとして、OperatorStateに戻る
        if (string.IsNullOrEmpty(ctx.RightSide))
        {
            ctx.Operator = btn;
            ctx.SubDisplayManager.ReplaceLast(btn.ToDisplayString());
            ctx.State = new OperatorState();
            return;
        }

        ctx.SubDisplayManager.Append(ctx.RightSide);
        ctx.SubDisplayManager.Append(btn.ToDisplayString());

        // 左側の入力とOperatorと右側の入力で計算を行い、結果を表示
        ctx.LeftSide = new Calculator(ctx.LeftSide, ctx.RightSide, ctx.Operator.Value).Run().ToString();
        ctx.RightSide = string.Empty; // RightSideはクリア
        ctx.MainDisplayManager.Update(ctx.LeftSide);
        ctx.Operator = btn;

        // OperatorStateに戻る
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.MainDisplayManager.Update(new Calculator(ctx.LeftSide, ctx.RightSide, ctx.Operator.Value).Run());
        ctx.SubDisplayManager.Clear();
        ctx.State = new AnswerState();
    }
}