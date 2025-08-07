namespace CalcLibCore.Tomida2
{
    using CalcLib;
    using CalcLibCore.Tomida2.Calc.Interpreter;
  using CalcLibCore.Tomida2.Calc.Strategy;

  /// <summary>
  /// Tomida2の計算サービス
  /// </summary>
  internal class CalcSvcTomida2 : ICalcSvc
    {
        public ICalcContext CreateContext()
        {
            return new CalcContextTomida2();
        }

    public void OnButtonClick(ICalcContext ctx, CalcButton btn)
    {
      var _ctx = ctx as CalcContextTomida2 ?? throw new ArgumentException("Invalid context type", nameof(ctx));
      var strategy = ButtonStrategyFactory.GetStrategy(btn);
      strategy.OnButtonClick(_ctx, btn);
    }
  }
}