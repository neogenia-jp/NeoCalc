using CalcLib;
using CalcLibCore.Tomida2.Calc.Interpreter;

namespace CalcLibCore.Tomida2
{
  internal class CalcContextTomida2 : CalcContext
  {
    private static readonly CalculatorParser parser = new CalculatorParser();
    string rowInput { get; set; } = string.Empty;
    IParseResult parseResult => 
      parser.Parse(rowInput);
 
  }
}