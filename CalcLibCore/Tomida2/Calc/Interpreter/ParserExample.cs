using System;

namespace CalcLibCore.Tomida2.Calc.Interpreter
{
  /// <summary>
  /// CalculatorParserの使用例を示すサンプルクラス
  /// </summary>
  public class ParserExample
  {
    public static void RunExamples()
    {
      var parser = new CalculatorParser();

      // 例1: 単一のオペランド
      Console.WriteLine("=== 単一のオペランド ===");
      TestParse(parser, "123");
      TestParse(parser, "123.45");
      TestParse(parser, ".5");
      TestParse(parser, "10.");

      // 例2: 演算子入力
      Console.WriteLine("\n=== 演算子入力 ===");
      TestParse(parser, "10+");
      TestParse(parser, "5.5*");
      TestParse(parser, "100/");

      // 例3: 進行中の入力
      Console.WriteLine("\n=== 進行中の入力 ===");
      TestParse(parser, "10+5");
      TestParse(parser, "3.14*2");
      TestParse(parser, "100/4");

      // 例3.1: 連続した演算（新機能）
      Console.WriteLine("\n=== 連続した演算 ===");
      TestParse(parser, "1+2+3");
      TestParse(parser, "10-5+2");
      TestParse(parser, "2*3*4");
      TestParse(parser, "100/4/5");
      TestParse(parser, "1+2*3");
      TestParse(parser, "10-5*2");

      // 例4: 完全な式
      Console.WriteLine("\n=== 完全な式 ===");
      TestEvaluate(parser, "10+5=");
      TestEvaluate(parser, "3.14*2=");
      TestEvaluate(parser, "100/4=");
      TestEvaluate(parser, "7-3=");

      // 例4.1: 連続した演算の完全な式（新機能）
      Console.WriteLine("\n=== 連続した演算の完全な式 ===");
      TestEvaluate(parser, "1+2+3=");
      TestEvaluate(parser, "10-5+2=");
      TestEvaluate(parser, "2*3*4=");
      TestEvaluate(parser, "100/4/5=");
      TestEvaluate(parser, "1+2*3=");
      TestEvaluate(parser, "10-5*2=");

      // 例5: エラーケース
      Console.WriteLine("\n=== エラーケース ===");
      TestParse(parser, "");
      TestParse(parser, "abc");
      TestParse(parser, "10++");
      TestParse(parser, "10/0=");
    }

    private static void TestParse(CalculatorParser parser, string input)
    {
      try
      {
        var result = parser.Parse(input);
        Console.WriteLine($"Parse(\"{input}\") = {FormatResult(result)}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Parse(\"{input}\") = Error: {ex.Message}");
      }
    }

    private static void TestEvaluate(CalculatorParser parser, string input)
    {
      try
      {
        var result = parser.Evaluate(input);
        Console.WriteLine($"Evaluate(\"{input}\") = {result}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Evaluate(\"{input}\") = Error: {ex.Message}");
      }
    }

    private static string FormatResult(object result)
    {
      return result switch
      {
        OperatorInput opInput => $"OperatorInput(Operand={opInput.Operand}, Operator={opInput.Operator})",
        ProgressInput progInput => $"ProgressInput(FirstOperand={progInput.FirstOperand}, Operator={progInput.Operator}, SecondOperand={progInput.SecondOperand})",
        CompleteExpression completeExpr => $"CompleteExpression(FirstOperand={completeExpr.FirstOperand}, Operator={completeExpr.Operator}, SecondOperand={completeExpr.SecondOperand}, IsComplete={completeExpr.IsComplete})",
        ChainedExpression chainedExpr => $"ChainedExpression(Operands=[{string.Join(", ", chainedExpr.Operands)}], Operators=[{string.Join(", ", chainedExpr.Operators)}], IsComplete={chainedExpr.IsComplete})",
        _ => result?.ToString() ?? "null"
      };
    }
  }
}
