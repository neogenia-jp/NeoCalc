namespace CalcLib.Mori
{
    internal class CalcContextExtend : CalcContext, ISubject
    {
        // 電卓とそれ以外のモードを切り替えるState
        private readonly Dictionary<string, IModeState> _modes;
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
                ["omikuji"] = new OmikujiState()
            };

            // 初期状態で電卓をクリア動作させる 直接Acceptを呼ぶ
            Accept(CalcButton.BtnClear);
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
            // モード切り替えボタンここで処理する
            if (btn.IsOmikuji())
            {
                _modeKey = "omikuji";
                Notify();
                return;
            }

            ModeResult result = Mode.Accept(btn);
            if (result.NextKey != null) _modeKey = result.NextKey;

			if (result.ForwardButton.HasValue)
			{
                result = Mode.Accept(result.ForwardButton.Value);
				if (result.NextKey != null) _modeKey = result.NextKey;
			}
            Notify();
        }
    }
}
