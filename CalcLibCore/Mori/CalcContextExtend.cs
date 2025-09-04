namespace CalcLib.Mori
{
    internal class CalcContextExtend : CalcContext, ISubject
    {
        // 電卓とそれ以外のモードを切り替えるState
        private readonly Dictionary<ModeKey, IModeState> _modes;
        // 現在のモードのキー文字列
        private ModeKey _modeKey = ModeKey.Calc;
        private IModeState Mode => _modes[_modeKey];
        private readonly List<IObserver> _observers = new();
        public DisplaySource DisplaySource => Mode.RowDisplay();
        public CalcContextExtend()
        {
            // モードの初期化
            _modes = new()
            {
                [ModeKey.Calc] = new CalcMode(),
                [ModeKey.Omikuji] = new OmikujiState(),
                [ModeKey.Stock] = new StockState()
            };

            // 既定モードでまずOnEnter
            Mode.OnEnter();

            // 初期状態で電卓をクリア動作させる 直接Acceptを呼ぶ
            Accept(CalcButton.BtnClear);
        }


        private void SwitchMode(ModeKey key)
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
            // 株価ボタンはトグル
            if (btn.IsStock())
            {
                var next = _modeKey == ModeKey.Stock ? ModeKey.Calc : ModeKey.Stock;
                SwitchMode(next);
                Notify();
                return;
            }
            // おみくじボタンはトグル
            if (btn.IsOmikuji())
            {
                var next = _modeKey == ModeKey.Omikuji ? ModeKey.Calc : ModeKey.Omikuji;
                SwitchMode(next);
                Notify();
                return;
            }

            // おみくじ中のClear/CEはcalcへ戻す（同ボタンを前進処理）
            if (_modeKey == ModeKey.Omikuji && (btn.IsClear() || btn.IsCE()))
            {
                SwitchMode(ModeKey.Calc);
                ProcessWithForward(btn);
                Notify();
                return;
            }

            // 通常処理
            ProcessWithForward(btn);
            Notify();
        }

        // モード切り替えとフォワードボタンの処理
        private void ProcessWithForward(CalcButton btn)
        {
            ModeResult result = Mode.Accept(btn);
            while (true)
            {
                if (result.Next != null)
                {
                    SwitchMode(result.Next.Value);
                }
                if (result.ForwardButton.HasValue)
                {
                    result = Mode.Accept(result.ForwardButton.Value);
                    continue;
                }
                break;
            }
        }
    }
}
