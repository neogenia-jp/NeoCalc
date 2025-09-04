using CalcLib;
using CalcLibCore.Tomida2.Calc.Interpreter;
using CalcLibCore.Tomida2.Calc.Strategy;
using CalcLibCore.Tomida2.Calc.implements;
using CalcLibCore.Tomida2.Calc.Memento;

namespace CalcLibCore.Tomida2
{
  internal class CalcContextTomida2 : CalcContext
  {
    private static readonly CalculatorParser parser = new();
    private static readonly DisplayTextImpl displayTextImpl = new();
    private static readonly SubDisplayTextImpl subDisplayTextImpl = new();
    private readonly CalcContextCaretaker _caretaker = new();
    string RawInput { get; set; } = string.Empty;
    bool IsResultDisplayed { get; set; } = false;
    
    /// <summary>
    /// 現在結果が表示されているかどうか
    /// </summary>
    public bool IsDisplayingResult => IsResultDisplayed;
    
    IParseResult ParseResult
    {
      get
      {
        if (string.IsNullOrEmpty(RawInput))
          return new DefaultParseResult();
        
        try
        {
          return parser.Parse(RawInput);
        }
        catch
        {
          // パーサーエラーが発生した場合はデフォルト値を返す
          return new DefaultParseResult();
        }
      }
    }

    public override string DisplayText 
    { 
      get 
      {
        return displayTextImpl.ToDisplay(RawInput, IsResultDisplayed, ParseResult);
      }
    }

    public override string SubDisplayText 
    { 
      get 
      {
        return subDisplayTextImpl.ToDisplay(RawInput, IsResultDisplayed, ParseResult);
      }
    }

    /// <summary>
    /// 入力に文字を追加します
    /// </summary>
    /// <param name="input">追加する文字</param>
    public void AppendInput(string input)
    {
      RawInput += input;
    }
    
    /// <summary>
    /// 結果を表示状態に設定します
    /// </summary>
    /// <param name="result">表示する結果</param>
    public void SetResult(string result)
    {
      RawInput = result;
      IsResultDisplayed = true;
    }
    
    /// <summary>
    /// 新しい数字入力で状態をリセットします
    /// </summary>
    public void StartNewCalculation()
    {
      if (IsResultDisplayed)
      {
        RawInput = string.Empty;
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
      RawInput = string.Empty;
    }

    /// <summary>
    /// 現在の入力内容を取得します
    /// </summary>
    public string GetCurrentInput()
    {
      return RawInput;
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

    /// <summary>
    /// 現在の状態をMementoとして保存します
    /// </summary>
    public void SaveState()
    {
      var memento = new CalcContextMemento(RawInput, IsResultDisplayed);
      _caretaker.SaveMemento(memento);
    }

    /// <summary>
    /// 最後に保存された状態に戻します（Undo）
    /// </summary>
    /// <returns>Undo実行できた場合はtrue</returns>
    public bool Undo()
    {
      var memento = _caretaker.GetLastMemento();
      if (memento == null)
        return false;

      RawInput = memento.RawInput;
      IsResultDisplayed = memento.IsResultDisplayed;
      return true;
    }

    /// <summary>
    /// Undo可能かどうかを確認します
    /// </summary>
    public bool CanUndo => _caretaker.CanUndo;

    /// <summary>
    /// 履歴をクリアします
    /// </summary>
    public void ClearHistory()
    {
      _caretaker.ClearHistory();
    }
  }
}