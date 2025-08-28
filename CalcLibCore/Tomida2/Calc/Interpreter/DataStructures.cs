using System;
using System.Collections.Generic;

namespace CalcLibCore.Tomida2.Calc.Interpreter
{
  /// <summary>
  /// パーサーの解析結果を表すインターフェース
  /// </summary>
  public interface IParseResult
  {
    /// <summary>
    /// 式を評価して結果を返す
    /// </summary>
    /// <returns>計算結果</returns>
    decimal Evaluate();

    /// <summary>
    /// 式がイコール（=）で終端されているかどうか
    /// </summary>
    bool IsTerminated { get; }
  }

  /// <summary>
  /// 空の入力状態のデフォルト解析結果
  /// </summary>
  public class DefaultParseResult : IParseResult
  {
    /// <summary>
    /// 空の入力では0を返す
    /// </summary>
    /// <returns>0</returns>
    public decimal Evaluate()
    {
      return 0;
    }

    /// <summary>
    /// 空の入力状態は終端されていない
    /// </summary>
    public bool IsTerminated => false;
  }

  /// <summary>
  /// 演算子が入力された状態を表すデータ構造
  /// </summary>
  public class OperatorInput : IParseResult
  {
    public decimal Operand { get; set; }
    public char Operator { get; set; }

    /// <summary>
    /// 演算子入力状態では第一オペランドをそのまま返す
    /// </summary>
    /// <returns>第一オペランドの値</returns>
    public decimal Evaluate()
    {
      return Operand;
    }

    /// <summary>
    /// 演算子入力状態は終端されていない
    /// </summary>
    public bool IsTerminated => false;
  }

  /// <summary>
  /// 進行中の入力状態を表すデータ構造
  /// </summary>
  public class ProgressInput : IParseResult
  {
    public decimal FirstOperand { get; set; }
    public char? Operator { get; set; }
    public decimal? SecondOperand { get; set; }

    /// <summary>
    /// 進行中の式を評価して結果を返す
    /// </summary>
    /// <returns>計算結果</returns>
    public decimal Evaluate()
    {
      if (Operator.HasValue && SecondOperand.HasValue)
      {
        // 二項演算
        return Operator.Value switch
        {
          '+' => FirstOperand + SecondOperand.Value,
          '-' => FirstOperand - SecondOperand.Value,
          '*' => FirstOperand * SecondOperand.Value,
          '/' => SecondOperand.Value != 0
              ? FirstOperand / SecondOperand.Value
              : throw new DivideByZeroException(),
          _ => throw new InvalidOperationException($"Unknown operator: {Operator.Value}")
        };
      }
      else
      {
        // 単一のオペランド
        return FirstOperand;
      }
    }

    /// <summary>
    /// 進行中の入力状態は終端されていない
    /// </summary>
    public bool IsTerminated => false;
  }

  /// <summary>
  /// 完全な式を表すデータ構造
  /// </summary>
  public class CompleteExpression : IParseResult
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

    /// <summary>
    /// 完全な式の場合、IsCompleteプロパティと同じ値を返す
    /// </summary>
    public bool IsTerminated => IsComplete;
  }

  /// <summary>
  /// 複数の演算操作をチェーンした式を表すデータ構造（例：1+2+3-4）
  /// </summary>
  public class ChainedExpression : IParseResult
  {
    public List<decimal> Operands { get; set; } = new List<decimal>();
    public List<char> Operators { get; set; } = new List<char>();
    public bool IsComplete { get; set; }

    /// <summary>
    /// 左結合で式を評価して結果を返す
    /// </summary>
    /// <returns>計算結果</returns>
    public decimal Evaluate()
    {
      if (Operands.Count == 0)
        throw new InvalidOperationException("No operands to evaluate");

      if (Operands.Count == 1)
        return Operands[0];

      if (Operators.Count != Operands.Count - 1)
        throw new InvalidOperationException("Operand and operator count mismatch");

      // 左結合で評価
      decimal result = Operands[0];
      for (int i = 0; i < Operators.Count; i++)
      {
        char op = Operators[i];
        decimal nextOperand = Operands[i + 1];

        result = op switch
        {
          '+' => result + nextOperand,
          '-' => result - nextOperand,
          '*' => result * nextOperand,
          '/' => nextOperand != 0 ? result / nextOperand : throw new DivideByZeroException(),
          _ => throw new InvalidOperationException($"Unknown operator: {op}")
        };
      }

      return result;
    }

    /// <summary>
    /// チェーンされた式の場合、IsCompleteプロパティと同じ値を返す
    /// </summary>
    public bool IsTerminated => IsComplete;

    /// <summary>
    /// 新しいオペランドを追加
    /// </summary>
    public void AddOperand(decimal operand)
    {
      Operands.Add(operand);
    }

    /// <summary>
    /// 新しい演算子を追加
    /// </summary>
    public void AddOperator(char op)
    {
      Operators.Add(op);
    }
  }
}
