using System;

namespace CalcLibCore.Tomida2.Calc.Interpreter
{
  /// <summary>
  /// 演算子が入力された状態を表すデータ構造
  /// </summary>
  public class OperatorInput
  {
    public decimal Operand { get; set; }
    public char Operator { get; set; }
  }

  /// <summary>
  /// 進行中の入力状態を表すデータ構造
  /// </summary>
  public class ProgressInput
  {
    public decimal FirstOperand { get; set; }
    public char? Operator { get; set; }
    public decimal? SecondOperand { get; set; }
  }

  /// <summary>
  /// 完全な式を表すデータ構造
  /// </summary>
  public class CompleteExpression
  {
    public decimal FirstOperand { get; set; }
    public char? Operator { get; set; }
    public decimal? SecondOperand { get; set; }
    public bool IsComplete { get; set; }

    /// <summary>
    /// 式を評価して結果を返す
    /// </summary>
    /// <returns>計算結果</returns>
    public decimal Evaluate()
    {
      if (!IsComplete || !Operator.HasValue)
        return FirstOperand;

      if (!SecondOperand.HasValue)
        throw new InvalidOperationException("Second operand is required for calculation");

      return Operator.Value switch
      {
        '+' => FirstOperand + SecondOperand.Value,
        '-' => FirstOperand - SecondOperand.Value,
        '*' => FirstOperand * SecondOperand.Value,
        '/' => SecondOperand.Value != 0 ? FirstOperand / SecondOperand.Value : throw new DivideByZeroException(),
        _ => throw new InvalidOperationException($"Unknown operator: {Operator.Value}")
      };
    }
  }
}
