using System;

namespace CalcLib.Yamamoto3.States;

internal class RightSideState : IState
{
    public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        if (ctx.RightSide.Length < 10)
        {
            ctx.RightSide += btn.ToString().Replace("Btn", string.Empty);
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

        // 左側の入力とOperatorと右側の入力で計算を行い、SubDisplayに結果を表示
        // RightSideStateに戻る
        ctx.State = new OperatorState();
    }

    public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
    {
        // TODO:
        // ctx.Operator.Execute(left, right); みたいな感じでできると、条件分岐なくなる
        // 左側の入力とOperatorと右側の入力で計算を行い、MainDisplayに結果を表示
        if(ctx.Operator == CalcButton.BtnPlus)
        {
            ctx.DisplayText = (int.Parse(ctx.LeftSide) + int.Parse(ctx.RightSide)).ToString();
        }
        else if(ctx.Operator == CalcButton.BtnMinus)
        {
            ctx.DisplayText = (int.Parse(ctx.LeftSide) - int.Parse(ctx.RightSide)).ToString();
        }
        else if(ctx.Operator == CalcButton.BtnMultiple)
        {
            ctx.DisplayText = (int.Parse(ctx.LeftSide) * int.Parse(ctx.RightSide)).ToString();
        }
        else if(ctx.Operator == CalcButton.BtnDivide)
        {
            if(int.TryParse(ctx.RightSide, out var right) && right != 0)
            {
                ctx.DisplayText = (int.Parse(ctx.LeftSide) / right).ToString();
            }
            else
            {
                ctx.DisplayText = "Error";
            }
        }
        // SubDisplayをクリア
        ctx.State = new AnswerState();
    }
}