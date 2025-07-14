using System;

namespace CalcLibCore.Tomida2
{
  /// <summary>
  /// 終端記号の基底クラス
  /// </summary>
  public abstract class TerminalExpression : IExpression
  {
    public abstract object Interpret(InterpreterContext context);
  }

  /// <summary>
  /// 数字（0-9）を表すクラス
  /// </summary>
  public class DigitExpression : TerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      context.SkipWhitespace();

      if (context.IsAtEnd)
        throw new InvalidOperationException("Unexpected end of input");

      char ch = context.CurrentChar;
      if (char.IsDigit(ch))
      {
        context.Advance();
        return ch;
      }

      throw new InvalidOperationException($"Expected digit, but found '{ch}'");
    }
  }

  /// <summary>
  /// 小数点（.）を表すクラス
  /// </summary>
  public class DotExpression : TerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      context.SkipWhitespace();

      if (context.IsAtEnd)
        throw new InvalidOperationException("Unexpected end of input");

      char ch = context.CurrentChar;
      if (ch == '.')
      {
        context.Advance();
        return ch;
      }

      throw new InvalidOperationException($"Expected '.', but found '{ch}'");
    }
  }

  /// <summary>
  /// 演算子（+, -, *, /）を表すクラス
  /// </summary>
  public class OperatorExpression : TerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      context.SkipWhitespace();

      if (context.IsAtEnd)
        throw new InvalidOperationException("Unexpected end of input");

      char ch = context.CurrentChar;
      if (ch == '+' || ch == '-' || ch == '*' || ch == '/')
      {
        context.Advance();
        return ch;
      }

      throw new InvalidOperationException($"Expected operator, but found '{ch}'");
    }
  }

  /// <summary>
  /// 終端記号（=）を表すクラス
  /// </summary>
  public class TerminatorExpression : TerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      context.SkipWhitespace();

      if (context.IsAtEnd)
        throw new InvalidOperationException("Unexpected end of input");

      char ch = context.CurrentChar;
      if (ch == '=')
      {
        context.Advance();
        return ch;
      }

      throw new InvalidOperationException($"Expected '=', but found '{ch}'");
    }
  }
}
