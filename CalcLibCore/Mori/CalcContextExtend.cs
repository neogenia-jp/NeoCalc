namespace CalcLib.Mori
{
    internal class CalcContextExtend : CalcContext
    {
        private readonly Calculator _calculator = new();
        private readonly DisplayObserver _displayObserver;

        public CalcContextExtend()
        {
            _displayObserver = new DisplayObserver(this);
            _calculator.Attach(_displayObserver);
            // 初期状態を反映
            _calculator.Notify();
        }

        public void Accept(CalcButton btn)
        {
            _calculator.Accept(btn);
        }
    }
}
