// 電卓以外のextraモード用State

namespace CalcLib.Mori
{
internal interface IModeState
{
    ModeResult Accept(CalcContextExtend context, CalcButton btn);
    DisplaySource RowDisplay();
}

internal class CalcMode : IModeState
{
    private static readonly IModeState singleton = new CalcMode();
    private CalcMode() { }
    public static IModeState GetInstance() => singleton;
    private readonly Calculator _calculator = new();

    public ModeResult Accept(CalcContextExtend context, CalcButton btn)
    {
       if (btn.IsOmikuji())
       {
          return ModeResult.SwitchMode(OmikujiState.GetInstance());
       }
       _calculator.Accept(btn);
       return ModeResult.Continue(this);
    }

    public DisplaySource RowDisplay()
    {
        return _calculator.RowDisplay();
    }
}

internal class OmikujiState : IModeState
{
    private readonly Omikuji _omikuji = new();
    private static readonly IModeState singleton = new OmikujiState();
    private OmikujiState() { }
    public static IModeState GetInstance() 
    {
        // 暫定初期化
        var instance = (OmikujiState)singleton;
        instance._omikuji.Init();
        return singleton;
    }

    public ModeResult Accept(CalcContextExtend context, CalcButton btn)
    {
		// クリア/CE/おみくじ → 電卓へ戻る（引き継ぎなし）
		if (btn.IsClear() || btn.IsCE() || btn.IsOmikuji())
		{
			_omikuji.Init();
			return ModeResult.SwitchMode(CalcMode.GetInstance());
		}
		// おみくじをひいたあとの数字の場合は、数字を引き継いで電卓へ
		if (_omikuji.HasSelected)
		{
			var fwd = btn.IsNumber() ? btn : (CalcButton?)null;
			_omikuji.Init();
			return ModeResult.SwitchMode(CalcMode.GetInstance(), fwd);
		}
		// おみじく未選択かつ 1〜4 おみくじにコンテキストとボタンを渡す
		if (btn.IsOmikujiSelect())
		{
			_omikuji.Accept(context, btn);
			return ModeResult.Continue(this);
		}

		// その他は無視して継続
		return ModeResult.Continue(this);
    }

    public DisplaySource RowDisplay()
    {
        return _omikuji.RowDisplay();
    }
}
}