using System;
using CalcLib;
using CalcLib.Yamamoto3.Extensions;

namespace CalcLib.Yamamoto3;

public class Calculator
{
    public string Left { get; }
    public string Right { get; }
    public CalcButton Operator { get; }

    public Calculator(string left, string right, CalcButton op)
    {
        if (!op.IsOperator())
        {
            // 演算子以外の場合はこのクラスは使えない
            throw new ArgumentException($"不正な演算子です {op}");
        }
        Left = left;
        Right = right;
        Operator = op;
    }

    public decimal Run()
    {
        decimal result = 0m;
        // TODO:
        // Operator.Execute(left, right); みたいな感じでできると、条件分岐なくなる
        // 左側の入力とOperatorと右側の入力で計算を行い、MainDisplayに結果を表示
        if (Operator == CalcButton.BtnPlus)
        {
            result = decimal.Parse(Left) + decimal.Parse(Right);
        }
        else if (Operator == CalcButton.BtnMinus)
        {
            result = decimal.Parse(Left) - decimal.Parse(Right);
        }
        else if (Operator == CalcButton.BtnMultiple)
        {
            result = decimal.Parse(Left) * decimal.Parse(Right);
        }
        else // if (Operator == CalcButton.BtnDivide)
        {
            if (decimal.TryParse(Right, out var right) && right != 0m)
            {
                result = decimal.Parse(Left) / right;
            }
            else
            {
                // TODO: 0除算した場合の仕様確認
                return 0m;
            }
        }
        return Math.Round(result, 13); // 小数部は13桁まで
    }
}

