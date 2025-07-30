using System;

namespace CalcLibCore.Tomida2.Calc.Interpreter
{
  /// <summary>
  /// BNFに基づく計算機パーサー
  /// </summary>
  public class CalculatorParser
  {
    /// <summary>
    /// 入力文字列を解析して状態を返す
    /// </summary>
    /// <param name="input">入力文字列</param>
    /// <returns>解析結果</returns>
    public object Parse(string input)
    {
      if (string.IsNullOrEmpty(input))
        throw new ArgumentException("Input cannot be null or empty", nameof(input));

      var context = new InterpreterContext(input);
      var stateExpr = new StateExpression();

      try
      {
        var result = stateExpr.Interpret(context);

        // 入力が完全に消費されたかチェック
        context.SkipWhitespace();
        if (!context.IsAtEnd)
          throw new InvalidOperationException($"Unexpected character at position {context.Position}: '{context.CurrentChar}'");

        return result;
      }
      catch (InvalidOperationException ex)
      {
        throw new ArgumentException($"Parse error: {ex.Message}", nameof(input), ex);
      }
    }

    /// <summary>
    /// 入力文字列を解析して式を評価する
    /// </summary>
    /// <param name="input">入力文字列</param>
    /// <returns>計算結果</returns>
    public decimal Evaluate(string input)
    {
      var result = Parse(input);

      if (result is CompleteExpression completeExpr)
      {
        return completeExpr.Evaluate();
      }
      else if (result is ProgressInput progressInput)
      {
        if (progressInput.Operator.HasValue && progressInput.SecondOperand.HasValue)
        {
          // 二項演算
          return progressInput.Operator.Value switch
          {
            '+' => progressInput.FirstOperand + progressInput.SecondOperand.Value,
            '-' => progressInput.FirstOperand - progressInput.SecondOperand.Value,
            '*' => progressInput.FirstOperand * progressInput.SecondOperand.Value,
            '/' => progressInput.SecondOperand.Value != 0
                ? progressInput.FirstOperand / progressInput.SecondOperand.Value
                : throw new DivideByZeroException(),
            _ => throw new InvalidOperationException($"Unknown operator: {progressInput.Operator.Value}")
          };
        }
        else
        {
          // 単一のオペランド
          return progressInput.FirstOperand;
        }
      }
      else if (result is OperatorInput)
      {
        throw new InvalidOperationException("Cannot evaluate incomplete expression with operator but no second operand");
      }

      throw new InvalidOperationException($"Unknown result type: {result.GetType()}");
    }

    /// <summary>
    /// 入力文字列が完全な式かどうかを判定
    /// </summary>
    /// <param name="input">入力文字列</param>
    /// <returns>完全な式かどうか</returns>
    public bool IsCompleteExpression(string input)
    {
      try
      {
        var result = Parse(input);
        return result is CompleteExpression;
      }
      catch
      {
        return false;
      }
    }
  }
}
