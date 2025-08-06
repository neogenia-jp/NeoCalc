namespace CalcLib.Mori
{
    internal class CalcContextExtend : CalcContext, ISubject
    {
        // 電卓とそれ以外のモードを切り替えるState
        private IModeState _mode = CalcMode.GetInstance();
        // private readonly DisplayObserver _displayObserver;
        private readonly List<IObserver> _observers = new();
        public DisplaySource DisplaySource => _mode.RowDisplay();
        public CalcContextExtend()
        {
            // _displayObserver = new DisplayObserver(this);
            // Attach(_displayObserver);
            // 初期状態を反映
            Notify();
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
            // モードの切り替えStateに委譲
            _mode = _mode.Accept(this, btn);
            Notify();
        }
    }
}
