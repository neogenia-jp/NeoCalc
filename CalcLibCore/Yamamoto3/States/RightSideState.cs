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
        ctx.LeftSide = Calc(ctx).ToString(); 
        ctx.RightSide = string.Empty; // RightSideはクリア
        ctx.DisplayText = string.Empty; // MainDisplayはクリア

        // OperatorStateに戻る
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        ctx.DisplayText = Calc(ctx).ToString(); 
        // SubDisplayをクリア
        ctx.SubDisplayText = string.Empty;
        ctx.State = new AnswerState();
    }
    private decimal Calc(CalcContextYamamoto3 ctx)
    {
        // TODO:
        // ctx.Operator.Execute(left, right); みたいな感じでできると、条件分岐なくなる
        // 左側の入力とOperatorと右側の入力で計算を行い、MainDisplayに結果を表示
        if (ctx.Operator == CalcButton.BtnPlus)
        {
            return decimal.Parse(ctx.LeftSide) + decimal.Parse(ctx.RightSide);
        }
        else if (ctx.Operator == CalcButton.BtnMinus)
        {
            return decimal.Parse(ctx.LeftSide) - decimal.Parse(ctx.RightSide);
        }
        else if (ctx.Operator == CalcButton.BtnMultiple)
        {
            return decimal.Parse(ctx.LeftSide) * decimal.Parse(ctx.RightSide);
        }
        else if (ctx.Operator == CalcButton.BtnDivide)
        {
            if (decimal.TryParse(ctx.RightSide, out var right) && right != 0m)
            {
                return decimal.Parse(ctx.LeftSide) / right;
            }
            else
            {
                // TODO: 0除算した場合の仕様確認
                return 0m;
            }
        }
        else
        {
            throw new InvalidOperationException("Unsupported operator");
        }
    }
}