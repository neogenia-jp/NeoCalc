using System;
using CalcLib.Yamamoto3.Extensions;

namespace CalcLib.Yamamoto3.States;

internal class RightSideState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.RightSide += btn.ToDisplayString();
        ctx.DisplayText += btn.ToDisplayString();
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
        ctx.Operator = btn;

        // OperatorStateに戻る
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // TODO: DisplayTextに変更があるたびにここを変更しないといけず苦しい。クラスにするなりして変更する箇所がわかりやすいようにしたい
        // decimal型で受け取って、それをいい感じにDisplayTextに変換して入れてくれるようなやつ
        ctx.DisplayText = new Calculator(ctx.LeftSide, ctx.RightSide, ctx.Operator.Value).Run().ToString("0.#############");
        // SubDisplayをクリア
        //ctx.SubDisplayText = string.Empty;
        ctx.State = new AnswerState();
    }
}