using System;
using CalcLib.Yamamoto3.Extensions;

namespace CalcLib.Yamamoto3.States;

internal class AnswerState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 内部状態をクリアする
        ctx.LeftSide = "";
        ctx.RightSide = "";
        ctx.LeftSide = btn.ToDisplayString();
        ctx.MainDisplayManager.Update(ctx.LeftSide);

        ctx.State = new LeftSideState();
    }

    public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // MainDisplayの内容を左辺およびSubDisplayにセット
        // サブディスプレイはカンマ付きだとダメだからGetRawTextというメソッド追加してカンマなしのものを
        // 設定するようにしたけど、これを意識しないといけないのが苦しい・・・
        ctx.LeftSide = ctx.MainDisplayManager.GetRawText();
        ctx.SubDisplayManager.Append(ctx.LeftSide);
        ctx.SubDisplayManager.Append(btn.ToDisplayString());
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特になし
    }

    public void InputBs(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // 特になし
    }
}