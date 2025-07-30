namespace CalcLibCore.Tomida2
{
    using CalcLib;
    using CalcLibCore.Tomida2.Calc.Interpreter;

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
      throw new NotImplementedException();
    }
  }
}