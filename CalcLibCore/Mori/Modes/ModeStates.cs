// 電卓以外のextraモード用State

using System;
using System.Text.RegularExpressions;
using CalcLib.Util;

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
		// おみくじをひいたあとの数字の場合は、数字を引き継いで電卓へ
        if (_omikuji.HasSelected)
        {
            var fwd = btn.IsNumber() ? btn : (CalcButton?)null;
            _omikuji.Init();
            return ModeResult.SwitchMode(ModeKey.Calc, fwd);
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

    internal class StockState : IModeState
    {
        private readonly CalcMode _calcMode;
        private readonly Stock _stock = new();

        public StockState(CalcMode calcMode)
        {
            _calcMode = calcMode;
        }

        public void OnEnter()
        {
            // Calcの表示から4桁コードを読み取り、株価取得
            _stock.Init(_calcMode.RowDisplay());
        }   

        public void OnLeave() { }

        public ModeResult Accept(CalcButton btn)
        {
            // 数字キーとクリア系は電卓モードへ
            if (btn.IsNumber() || btn.IsClear() || btn.IsCE() || btn.IsBS())
            {
                return ModeResult.SwitchMode(ModeKey.Calc, btn);
            }

            // = キーは現在表示中の指標を再取得
            if (btn.IsEqual())
            {
                _stock.Refresh();
                return ModeResult.Continue();
            }

            // - でNYダウ表示
            if (btn == CalcButton.BtnMinus)
            {
                _stock.Accept(btn);
                return ModeResult.Continue();
            }
            // + で日経平均表示
            if (btn == CalcButton.BtnPlus)
            {
                _stock.Accept(btn);
                return ModeResult.Continue();
            }
            // その他のボタンはモード継続
            return ModeResult.Continue();
        }

        public DisplaySource RowDisplay()
        {
            return _stock.RowDisplay();
        }
    }
}
