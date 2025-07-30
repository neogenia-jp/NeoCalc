using System;
using System.Text;

namespace CalcLibCore.Tomida2.Calc.Interpreter
{
  /// <summary>
  /// 非終端記号の基底クラス
  /// </summary>
  public abstract class NonTerminalExpression : IExpression
  {
    public abstract object Interpret(InterpreterContext context);
  }

  /// <summary>
  /// &lt;digits&gt; ::= &lt;digit&gt; | &lt;digits&gt; &lt;digit&gt;
  /// </summary>
  public class DigitsExpression : NonTerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      var digits = new StringBuilder();
      var digitExpr = new DigitExpression();

      try
      {
        // 最初の数字を読み取る
        char firstDigit = (char)digitExpr.Interpret(context);
        digits.Append(firstDigit);

        // 続く数字を読み取る
        while (!context.IsAtEnd)
        {
          context.SkipWhitespace();
          if (context.IsAtEnd || !char.IsDigit(context.CurrentChar))
            break;

          char nextDigit = (char)digitExpr.Interpret(context);
          digits.Append(nextDigit);
        }

        return digits.ToString();
      }
      catch (InvalidOperationException)
      {
        throw new InvalidOperationException("Expected at least one digit");
      }
    }
  }

  /// <summary>
  /// &lt;operand&gt; ::= &lt;digits&gt; | &lt;dot&gt; &lt;digits&gt; | &lt;digits&gt; &lt;dot&gt; | &lt;digits&gt; &lt;dot&gt; &lt;digits&gt;
  /// </summary>
  public class OperandExpression : NonTerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      var operand = new StringBuilder();
      int savedPosition = context.Position;

      try
      {
        // Case 1: <digits> <dot> <digits>
        try
        {
          var digits1 = new DigitsExpression();
          var dot = new DotExpression();
          var digits2 = new DigitsExpression();

          string integerPart = (string)digits1.Interpret(context);
          char dotChar = (char)dot.Interpret(context);
          string fractionalPart = (string)digits2.Interpret(context);

          operand.Append(integerPart).Append(dotChar).Append(fractionalPart);
          return decimal.Parse(operand.ToString());
        }
        catch (InvalidOperationException)
        {
          context.SetPosition(savedPosition);
        }

        // Case 2: <digits> <dot>
        try
        {
          var digitsExpr2 = new DigitsExpression();
          var dot = new DotExpression();

          string integerPart = (string)digitsExpr2.Interpret(context);
          char dotChar = (char)dot.Interpret(context);

          operand.Append(integerPart).Append(dotChar);
          return decimal.Parse(operand.ToString());
        }
        catch (InvalidOperationException)
        {
          context.SetPosition(savedPosition);
        }

        // Case 3: <dot> <digits>
        try
        {
          var dot = new DotExpression();
          var digitsExpr3 = new DigitsExpression();

          char dotChar = (char)dot.Interpret(context);
          string fractionalPart = (string)digitsExpr3.Interpret(context);

          operand.Append(dotChar).Append(fractionalPart);
          return decimal.Parse(operand.ToString());
        }
        catch (InvalidOperationException)
        {
          context.SetPosition(savedPosition);
        }

        // Case 4: <digits>
        var digitsExpr = new DigitsExpression();
        string digits = (string)digitsExpr.Interpret(context);
        return decimal.Parse(digits);
      }
      catch (InvalidOperationException)
      {
        throw new InvalidOperationException("Expected operand");
      }
    }
  }

  /// <summary>
  /// &lt;exp_input_operator&gt; ::= &lt;operand&gt; &lt;operator&gt;
  /// </summary>
  public class ExpInputOperatorExpression : NonTerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      var operand = new OperandExpression();
      var operatorExpr = new OperatorExpression();

      try
      {
        decimal operandValue = (decimal)operand.Interpret(context);
        char operatorChar = (char)operatorExpr.Interpret(context);

        return new OperatorInput
        {
          Operand = operandValue,
          Operator = operatorChar
        };
      }
      catch (InvalidOperationException)
      {
        throw new InvalidOperationException("Expected operand followed by operator");
      }
    }
  }

  /// <summary>
  /// &lt;exp_input_progress&gt; ::= &lt;operand&gt; | &lt;operand&gt; &lt;operator&gt; &lt;operand&gt;
  /// </summary>
  public class ExpInputProgressExpression : NonTerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      int savedPosition = context.Position;

      try
      {
        // まず最初のオペランドを読み取る
        var firstOperand = new OperandExpression();
        decimal firstOperandValue = (decimal)firstOperand.Interpret(context);

        // 空白をスキップ
        context.SkipWhitespace();

        // 演算子があるかチェック
        if (!context.IsAtEnd && (context.CurrentChar == '+' || context.CurrentChar == '-' || 
                                context.CurrentChar == '*' || context.CurrentChar == '/'))
        {
          // 演算子を読み取る
          var operatorExpr = new OperatorExpression();
          char operatorChar = (char)operatorExpr.Interpret(context);

          // 空白をスキップ
          context.SkipWhitespace();

          // 2番目のオペランドがあるかチェック
          if (!context.IsAtEnd && (char.IsDigit(context.CurrentChar) || context.CurrentChar == '.'))
          {
            // 2番目のオペランドを読み取る（完全な進行中入力）
            var secondOperand = new OperandExpression();
            decimal secondOperandValue = (decimal)secondOperand.Interpret(context);

            return new ProgressInput
            {
              FirstOperand = firstOperandValue,
              Operator = operatorChar,
              SecondOperand = secondOperandValue
            };
          }
          else
          {
            // 演算子はあるが2番目のオペランドがない場合は失敗
            // この場合は ExpInputOperatorExpression で処理されるべき
            throw new InvalidOperationException("Second operand expected after operator");
          }
        }
        else
        {
          // 演算子がない場合は単一のオペランド
          return new ProgressInput
          {
            FirstOperand = firstOperandValue,
            Operator = null,
            SecondOperand = null
          };
        }
      }
      catch (InvalidOperationException)
      {
        context.SetPosition(savedPosition);
        throw new InvalidOperationException("Expected operand or complete progress input");
      }
    }
  }

  /// <summary>
  /// &lt;expression&gt; ::= &lt;exp_input_operator&gt; &lt;terminator&gt; | &lt;exp_input_progress&gt; &lt;terminator&gt;
  /// </summary>
  public class ExpressionExpression : NonTerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      int savedPosition = context.Position;

      try
      {
        // Case 1: <exp_input_operator> <terminator>
        try
        {
          var expInputOp = new ExpInputOperatorExpression();
          var terminator = new TerminatorExpression();

          var operatorInput = (OperatorInput)expInputOp.Interpret(context);
          char terminatorChar = (char)terminator.Interpret(context);

          return new CompleteExpression
          {
            FirstOperand = operatorInput.Operand,
            Operator = operatorInput.Operator,
            SecondOperand = null,
            IsComplete = true
          };
        }
        catch (InvalidOperationException)
        {
          context.SetPosition(savedPosition);
        }

        // Case 2: <exp_input_progress> <terminator>
        var expInputProgress = new ExpInputProgressExpression();
        var terminatorExpr = new TerminatorExpression();

        var progressInput = (ProgressInput)expInputProgress.Interpret(context);
        char termChar = (char)terminatorExpr.Interpret(context);

        return new CompleteExpression
        {
          FirstOperand = progressInput.FirstOperand,
          Operator = progressInput.Operator,
          SecondOperand = progressInput.SecondOperand,
          IsComplete = true
        };
      }
      catch (InvalidOperationException)
      {
        throw new InvalidOperationException("Expected expression followed by terminator");
      }
    }
  }

  /// <summary>
  /// &lt;state&gt; ::= &lt;expression&gt; | &lt;exp_input_operator&gt; | &lt;exp_input_progress&gt;
  /// </summary>
  public class StateExpression : NonTerminalExpression
  {
    public override object Interpret(InterpreterContext context)
    {
      int savedPosition = context.Position;

      try
      {
        // Case 1: <expression> (最も具体的 - 末尾に=がある完全な式)
        try
        {
          var expression = new ExpressionExpression();
          return expression.Interpret(context);
        }
        catch (InvalidOperationException)
        {
          context.SetPosition(savedPosition);
        }

        // Case 2: <exp_input_progress> (中程度 - operand + operator + operand)
        try
        {
          var expInputProgress = new ExpInputProgressExpression();
          return expInputProgress.Interpret(context);
        }
        catch (InvalidOperationException)
        {
          context.SetPosition(savedPosition);
        }

        // Case 3: <exp_input_operator> (最も一般的 - operand + operator)
        var expInputOp = new ExpInputOperatorExpression();
        return expInputOp.Interpret(context);
      }
      catch (InvalidOperationException)
      {
        throw new InvalidOperationException("Expected valid state expression");
      }
    }
  }
}
