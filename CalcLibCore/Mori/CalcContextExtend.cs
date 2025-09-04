namespace CalcLib.Mori
{
    internal class CalcContextExtend : CalcContext, ISubject
    {
        // 電卓とそれ以外のモードを切り替えるState
        private readonly Dictionary<string, IModeState> _modes;
        // 現在のモードのキー文字列
        private string _modeKey = "calc";
        private IModeState Mode => _modes[_modeKey];
        private readonly List<IObserver> _observers = new();
        public DisplaySource DisplaySource => Mode.RowDisplay();
        public CalcContextExtend()
        {
            // モードの初期化
            _modes = new()
            {
                ["calc"] = new CalcMode(),
                ["omikuji"] = new OmikujiState(),
                ["stock"] = new StockState()
            };

            // 既定モードでまずOnEnter
            Mode.OnEnter();

            // 初期状態で電卓をクリア動作させる 直接Acceptを呼ぶ
            Accept(CalcButton.BtnClear);
        }

        private void SwitchMode(string key)
        {
            if (_modeKey == key) return;
            // 現在のモードでOnLeave処理
            Mode.OnLeave();
            _modeKey = key;
            // 新しいモードでOnEnter処理
            Mode.OnEnter();
        }

        // サブジェクト用 オブザーバー登録
        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer)) 
            {
                _observers.Add(observer);
            }
        }

        // サブジェクト用 オブザーバー解除
        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        // サブジェクト用 通知
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        public void Accept(CalcButton btn)
        {
            // 暫定 株価ボタンはトグル
            if (btn.IsStock())
            {
                var next = _modeKey == "stock" ? "calc" : "stock";
                SwitchMode(next);
                Notify();
                return;
            }
            // 暫定 おみくじボタンはトグル
            if (btn.IsOmikuji())
            {
                var next = _modeKey == "omikuji" ? "calc" : "omikuji";
                SwitchMode(next);
                Notify();
                return;
            }

            // おみくじ中のClear/CEはcalcへ戻す
            if (_modeKey == "omikuji" && (btn.IsClear() || btn.IsCE()))
            {
                SwitchMode("calc");
                var post = Mode.Accept(btn); // ボタンをcalcに委譲 ModeResult
                if (post.NextKey != null) { SwitchMode(post.NextKey); }
                if (post.ForwardButton.HasValue)
                {
                    post = Mode.Accept(post.ForwardButton.Value);
                    if (post.NextKey != null) { SwitchMode(post.NextKey); }
                }
                Notify();
                return;
            }

            ModeResult result = Mode.Accept(btn);
            if (result.NextKey != null) { SwitchMode(result.NextKey); }

				if (result.ForwardButton.HasValue)
				{
                result = Mode.Accept(result.ForwardButton.Value);
					if (result.NextKey != null) { SwitchMode(result.NextKey); }
				}
            Notify();
        }
    }
}
