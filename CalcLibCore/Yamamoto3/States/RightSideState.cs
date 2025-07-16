using System;

namespace CalcLib.Yamamoto3.States;

internal class RightSideState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        if (ctx.RightSide.Length < 10)
        {
            ctx.RightSide += btn.ToString().Replace("Btn", string.Empty);
            ctx.DisplayText += btn.ToString().Replace("Btn", string.Empty);
        }
        ctx.State = new RightSideState();
    }

    public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // まだ右側の入力がない場合はOperatorが変更されたとして、OperatorStateに戻る
        if (string.IsNullOrEmpty(ctx.RightSide))
        {
            ctx.Operator = btn;
            ctx.State = new OperatorState();
            return;
        }

        // 左側の入力とOperatorと右側の入力で計算を行い、結果を表示
        ctx.LeftSide = new Calculator(ctx.LeftSide, ctx.RightSide, ctx.Operator.Value).Run().ToString();
        ctx.RightSide = string.Empty; // RightSideはクリア
        ctx.DisplayText = string.Empty; // MainDisplayはクリア

        // OperatorStateに戻る
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.DisplayText = new Calculator(ctx.LeftSide, ctx.RightSide, ctx.Operator.Value).Run().ToString();
        // SubDisplayをクリア
        //ctx.SubDisplayText = string.Empty;
        ctx.State = new AnswerState();
    }
}