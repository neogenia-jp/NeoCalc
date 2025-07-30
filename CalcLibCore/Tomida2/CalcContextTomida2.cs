using CalcLib;
using CalcLibCore.Tomida2.Calc.Interpreter;
using CalcLibCore.Tomida2.Calc.Strategy;

namespace CalcLibCore.Tomida2
{
  internal class CalcContextTomida2 : CalcContext
  {
    private static readonly CalculatorParser parser = new CalculatorParser();
    string RowInput { get; set; } = string.Empty;
    IParseResult ParseResult =>
      parser.Parse(RowInput);

    public override string DisplayText { get => ParseResult.Evaluate().ToString(); set => base.DisplayText = value; }

    /// <summary>
    /// 入力に文字を追加します
    /// </summary>
    /// <param name="input">追加する文字</param>
    public void AppendInput(string input)
    {
      RowInput += input;
    }

    /// <summary>
    /// 入力をクリアします
    /// </summary>
    public void ClearInput()
    {
      RowInput = string.Empty;
    }

    /// <summary>
    /// 現在の入力内容を取得します
    /// </summary>
    public string GetCurrentInput()
    {
      return RowInput;
    }

    /// <summary>
    /// ボタンクリックを処理します
    /// </summary>
    /// <param name="button">クリックされたボタン</param>
    public void HandleButtonClick(CalcButton button)
    {
      if (ButtonStrategyFactory.IsSupported(button))
      {
        var strategy = ButtonStrategyFactory.GetStrategy(button);
        strategy.OnButtonClick(this, button);
      }
      else
      {
        throw new System.NotSupportedException($"Button {button} is not supported");
      }
    }
  }
}