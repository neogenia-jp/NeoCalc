using System;

namespace CalcLibCore.Tomida2.Calc.Interpreter
{
  /// <summary>
  /// Interpreterパターンの基本インターフェース
  /// </summary>
  public interface IExpression
  {
    /// <summary>
    /// 式を解釈して結果を返す
    /// </summary>
    /// <param name="context">解釈に必要なコンテキスト情報</param>
    /// <returns>解釈結果</returns>
    object Interpret(InterpreterContext context);
  }
}
