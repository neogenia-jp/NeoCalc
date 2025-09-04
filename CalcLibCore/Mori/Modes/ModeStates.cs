// 電卓以外のextraモード用State

namespace CalcLib.Mori
{
internal interface IModeState
{
    void OnEnter();
    void OnLeave();
    ModeResult Accept(CalcButton btn);
    DisplaySource RowDisplay();
}

internal class CalcMode : IModeState
{
    private readonly Calculator _calculator = new();
    public void OnEnter() { }
    public void OnLeave() { }

    public ModeResult Accept(CalcButton btn)
    {
       _calculator.Accept(btn);
        return ModeResult.Continue();
    }

    public DisplaySource RowDisplay()
    {
        return _calculator.RowDisplay();
    }
}

internal class OmikujiState : IModeState
{
    private readonly Omikuji _omikuji = new();
    public void OnEnter() { _omikuji.Init(); }
    public void OnLeave() { _omikuji.Init(); }
    public ModeResult Accept(CalcButton btn)
    {
		// クリア/CE/おみくじ → 電卓へ戻る（引き継ぎなし）
		if (btn.IsClear() || btn.IsCE() || btn.IsOmikuji())
		{
			_omikuji.Init();
			return ModeResult.SwitchMode("calc");
		}
		// おみくじをひいたあとの数字の場合は、数字を引き継いで電卓へ
		if (_omikuji.HasSelected)
		{
			var fwd = btn.IsNumber() ? btn : (CalcButton?)null;
			_omikuji.Init();
			return ModeResult.SwitchMode("calc", fwd);
		}
		// おみじく未選択かつ 1〜4 おみくじにコンテキストとボタンを渡す
		if (btn.IsOmikujiSelect())
		{
			_omikuji.Accept(btn);
			return ModeResult.Continue();
		}

		// その他は無視して継続
		return ModeResult.Continue();
    }

    public DisplaySource RowDisplay()
    {
        return _omikuji.RowDisplay();
    }
}
}

