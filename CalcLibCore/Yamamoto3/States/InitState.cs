using System;

namespace CalcLib.Yamamoto3.States;

internal class InitState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        if (ctx.LeftSide.Length < 10)
        {
            // TODO: Enumに拡張メソッド定義して1とか2とか返すようにしたい
            ctx.LeftSide += btn.ToString().Replace("Btn", string.Empty);
            ctx.DisplayText += btn.ToString().Replace("Btn", string.Empty);
        }
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