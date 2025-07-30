using CalcLib;
using CalcLibCore.Tomida2.Calc.Interpreter;
using CalcLibCore.Tomida2.Calc.Strategy;

namespace CalcLibCore.Tomida2
{
  internal class CalcContextTomida2 : CalcContext
  {
    private static readonly CalculatorParser parser = new();
    string RowInput { get; set; } = string.Empty;
    bool IsResultDisplayed { get; set; } = false;
    
    /// <summary>
    /// 現在結果が表示されているかどうか
    /// </summary>
    public bool IsDisplayingResult => IsResultDisplayed;
    
    IParseResult ParseResult =>
      parser.Parse(RowInput);

    public override string DisplayText 
    { 
      get 
      {
        // 結果表示中はそのまま返す
        if (IsResultDisplayed)
        {
          return RowInput;
        }
        
        try 
        {
          var result = ParseResult.Evaluate();
          // 小数点以下の桁数を制限（13桁）
          if (result != Math.Floor(result))
          {
            return Math.Round(result, 13).ToString();
          }
          return result.ToString();
        }
        catch 
        {
          // パーサーエラーが発生した場合は、現在の入力をそのまま表示
          return RowInput;
        }
      }
      set => base.DisplayText = value; 
    }

    /// <summary>
    /// 入力に文字を追加します
    /// </summary>
    /// <param name="input">追加する文字</param>
    public void AppendInput(string input)
    {
      RowInput += input;
    }
    
    /// <summary>
    /// 結果を表示状態に設定します
    /// </summary>
    /// <param name="result">表示する結果</param>
    public void SetResult(string result)
    {
      RowInput = result;
      IsResultDisplayed = true;
    }
    
    /// <summary>
    /// 新しい数字入力で状態をリセットします
    /// </summary>
    public void StartNewCalculation()
    {
      if (IsResultDisplayed)
      {
        RowInput = string.Empty;
        IsResultDisplayed = false;
      }
    }
    
    /// <summary>
    /// 結果から継続計算を開始します
    /// </summary>
    public void ContinueFromResult()
    {
      if (IsResultDisplayed)
      {
        IsResultDisplayed = false;
      }
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