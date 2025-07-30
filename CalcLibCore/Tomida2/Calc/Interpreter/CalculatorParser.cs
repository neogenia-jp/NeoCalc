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
    public IParseResult Parse(string input)
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

        return (IParseResult)result;
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
      return result.Evaluate();
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
